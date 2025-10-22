using System.Collections.Generic;
using UnityEngine;
namespace solsyssim {
    // This script is responsible for managing UI elements
    //It uses linerender to draw the orbit lines
    [RequireComponent(typeof(LineRenderer))]
    public class OrbitalUI : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private OrbitalBody orbitalBody;
        //Higher value means smoother orbit lines 
        private int resolution = 80;

        private List<Vector3> points = new List<Vector3>();
        public LineRenderer Line;
        //ises the getposition function in orbitalbody to calculate the points of the orbital path
        public void GetPoints()
        {
            //clears existing points just in case to avoid any errors
            points.Clear();

            Vector3 centre = orbitalBody._stellarParent.transform.position;

            float orbitFraction = 1f / resolution;
            //iterates through based on the resolution and adds a specific number of points corresponding to that resolution
            for (int i = 0; i < resolution; i++)
            {
                float meanAnomaly = i * orbitFraction * 2f * Mathf.PI;
                Vector3 pointPosition = orbitalBody.GetPosition(meanAnomaly, "getpoints");
                // Debug.Log(pointPosition);
                points.Add(centre + pointPosition);
            }
            lineRenderer.positionCount = points.Count;
            UpdateLines();
        }


        //resets to defualt settings
        private void Reset()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.loop = true;

            orbitalBody = GetComponent<OrbitalBody>(); 
        }
        
        private void Start()
        {
            orbitalBody = GetComponent<OrbitalBody>();
            orbitalBody.CalcSemiConstants(); 
            //skips adding orbit lines to sun as it is not needed
            if (orbitalBody.name != "Sun") {
                Reset();
                GetPoints();
                UpdateLines();
                //GenerateMeshCollider();
            }
            
        }

        private void Update() {
            GetPoints();
        }

        //updtes the orbital line 
        private void UpdateLines()
        {
            for (int i = 0; i < resolution; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }

            // Calculate the distance from the camera to the object
            float distance = Vector3.Distance(Camera.main.transform.position, transform.position);

            // Adjust the line width based on the distance
            float lineWidth = Mathf.Clamp(distance / 100, 0.1f, 2f); // Adjust the divisor and limits as needed

            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
        }

        //scrapped feature

        // public void GenerateMeshCollider()
        // {
        //     MeshCollider collider = GetComponent<MeshCollider>();
        //     if (collider == null)
        //     {
        //         collider = orbitalBody._stellarParent.gameObject.AddComponent<MeshCollider>();
        //     }

        //     Mesh mesh = new Mesh();

        //     lineRenderer.BakeMesh(mesh, Camera.main, false);
        //     collider.sharedMesh = mesh;

        // }

    }
}


