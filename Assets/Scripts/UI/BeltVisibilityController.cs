using UnityEngine;

namespace solsyssim {

    // Controls the visibility of the asteroid belt based on the distance from the main camera.

    public class BeltVisibilityController : MonoBehaviour
    {
        public BeltSpawner specificBeltSpawner; // Reference to the BeltSpawner to control visibility.
        public float thresholdDistance = 100f; // Distance threshold for toggling visibility.
        private Camera mainCamera; // Reference to the main camera in the scene.

        private void Start()
        {
            mainCamera = Camera.main; // Initialize the main camera reference.
        }

        private void Update()
        {
            // Calculate the distance between the camera and the belt spawner.
            float distance = Vector3.Distance(mainCamera.transform.position, specificBeltSpawner.transform.position);
            // Toggle the visibility based on the distance exceeding the threshold.
            specificBeltSpawner.gameObject.SetActive(distance > thresholdDistance);
        }
    }
}