using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace solsyssim {
    /// <summary>
    /// The BeltSpawner script is responsible for generating an asteroid belt within specified parameters.
    /// It creates asteroids with varied positions, scales, and rotations to simulate a belt around a central point.
    /// </summary>
    public class BeltSpawner : MonoBehaviour
    {
        [Header("Spawner Settings")]
        //list of prefabs for variation in asteroids
        public GameObject[] cubePrefabs;
        public int cubeDensity;
        public float eccentricity; 
        public int seed; 
        public float innerRadius; 
        public float outerRadius; 
        public float height; 
        public bool rotatingClockwise;
        //asteroid size range
        public float minScale = 0.1f; 
        public float maxScale = 0.2f; 
        public float maxInclination = 30f;

        // rotation and orbit speeds
        [Header("Asteroid Settings")]
        public float minOrbitSpeed; 
        public float maxOrbitSpeed; 
        public float minRotationSpeed; 
        public float maxRotationSpeed; 

        // Internal variables for calculations.
        private Vector3 localPosition;
        private Vector3 worldOffset;
        private Vector3 worldPosition;
        private float randomRadius;
        private float randomRadian;
        private float x, y, z;

        private void Start()
        {
            Random.InitState(seed);

            for (int i = 0; i < cubeDensity; i++)
            {
                do
                {
                    randomRadius = Random.Range(innerRadius, outerRadius);
                    randomRadian = Random.Range(0, (2 * Mathf.PI));

                    y = Random.Range(-(height / 2), (height / 2));
                    x = (1 + eccentricity) * randomRadius * Mathf.Cos(randomRadian);
                    z = randomRadius * Mathf.Sin(randomRadian);
                }
                while (float.IsNaN(z) && float.IsNaN(x));

                float inclination = Random.Range(-maxInclination, maxInclination);
                Quaternion rotation = Quaternion.Euler(inclination, 0, 0);
                localPosition = rotation * new Vector3(x, y, z);
                worldOffset = transform.rotation * localPosition;
                worldPosition = transform.position + worldOffset;

                GameObject selectedPrefab = cubePrefabs[Random.Range(0, cubePrefabs.Length)];
                GameObject _asteroid = Instantiate(selectedPrefab, worldPosition, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

                float scale = Random.Range(minScale, maxScale);
                _asteroid.transform.localScale = new Vector3(scale, scale, scale);

                _asteroid.AddComponent<BeltObject>().SetupBeltObject(Random.Range(minOrbitSpeed, maxOrbitSpeed), Random.Range(minRotationSpeed, maxRotationSpeed), gameObject, rotatingClockwise);
                _asteroid.transform.SetParent(transform);
            }
        }
    }
}