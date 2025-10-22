using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace solsyssim {
    // this script uss the beltspawner script to construct a whole asterid belt as one obejct in unity
    public class BeltObject : MonoBehaviour
    {
        // Serialized fields for object properties
        [SerializeField] private float orbitSpeed;
        [SerializeField] private GameObject parent;
        [SerializeField] private bool rotationClockwise;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Vector3 rotationDirection;

        // Initializes the belt object with given parameters
        public void SetupBeltObject(float _speed, float _rotationSpeed, GameObject _parent, bool _rotateClockwise)
        {
            orbitSpeed = _speed;
            rotationSpeed = _rotationSpeed;
            parent = _parent;
            rotationClockwise = _rotateClockwise;
            rotationDirection = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }

        // Handles orbit and self-rotation of the object
        private void Update()
        {
            float deltaTime = (float)SpaceTime.Instance.DeltatTime;
            Vector3 rotationAxis = rotationClockwise ? parent.transform.up : -parent.transform.up;
            transform.RotateAround(parent.transform.position, rotationAxis, orbitSpeed * deltaTime);
            transform.Rotate(rotationDirection, rotationSpeed * deltaTime);
        }
    }
}