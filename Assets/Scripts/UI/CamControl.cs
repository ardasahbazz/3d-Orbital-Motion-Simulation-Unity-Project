using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


namespace solsyssim {
    //Manages camera control including rotation, translation, and focus selection within the simulation.    
    public class CamControl : MonoBehaviour {

        [SerializeField] private Camera _cam;
        private Transform _camPole;

        private CamAnimator _camAnimator;
        

        // cam control togle
        private bool _userControl = false;

        // first menu related variables
        private bool _initializeCam = false;

        // audio reference and variables
        [SerializeField] private AudioClip _selectionSound;

        // cam focus variable and event action
        public Transform _selectedBody;
        private TextMeshProUGUI _focus;

        public event Action<Transform> NewFocus;

        const float CamSpeed = 25f;
        const float ZoomSpeed = 2f;
        const float ZoomMin = -0.1f;
        const float ZoomMax = -800.0f;
        const float RayLength = 1000000f;

        private void Start() {

            _cam = Camera.main;
            _camPole = _cam.transform.parent;
            _focus = GetComponent<TextMeshProUGUI>();

            _camAnimator = GetComponent<CamAnimator>();

            ControlIntentions.Instance.CamRotation += RotateCam;
            ControlIntentions.Instance.CamTranslation += TranslateCam;
            ControlIntentions.Instance.FocusSelection += CheckSelection;
            
            _selectedBody = transform;
            _selectedBody = GameObject.Find("Sun").transform.GetChild(0);

            ControlIntentions.Instance.MenuCall += PanCam;

            // Set the first body transform to istelf (null) to avoid error with cam zoom function.
            
        }

        // private void OnDestroy() {
        //     ControlIntentions.Instance.CamRotation -= RotateCam;
        //     ControlIntentions.Instance.CamTranslation -= TranslateCam;
        //     ControlIntentions.Instance.FocusSelection -= CheckSelection;
        // }

        // update the cams pole position to the focus body position
        private void Update() {
            // We always set the cam axis position to the selected celestial object's position.
            transform.position = Vector3.Lerp(transform.position, _selectedBody.position, 0.3f);
        }

        // the animatino when first starting the game, it is triggerred by an event call
        // we simply unregister the method so it is not called again
		public void PanCam(bool unused){
			if(_camAnimator != null)
				_camAnimator.NextAnim (new CamAnimation(_cam.transform, camFinalDepthZPos:-20f, poleFinalVerticalXRot:25f));

            ControlIntentions.Instance.MenuCall -= PanCam;
		}

        // stop any ongoing animation by simply creating a new void one
        private void StopAnimation(){
			if (_camAnimator!= null && _camAnimator.IsAnimating)
				_camAnimator.NextAnim ();
		}

        /// <summary>        
        /// Listen to control event and manages the orbital rotation of the cam.
        /// </summary>        
        private void RotateCam(string axis, float dir) {
            // Rotation of the cam around the center horizontally (on the y axis of Axis).
            if (axis == "horizontal")
                transform.Rotate(new Vector3(0, dir * CamSpeed * Time.deltaTime, 0));

            // Rotation of the cam around the center vertically (on the x axis of Pole).
            else if (axis == "vertical")
                _camPole.Rotate(new Vector3(dir * CamSpeed * Time.deltaTime, 0, 0));
            
            // And just to be sure
            else
                Debug.LogWarning("Cam rotation called on unknown axis: " + axis);

            StopAnimation();
        }
        private void TranslateCam(float translation) {
            // Uses the actual distance from the cam to the focus body as a factor in how fast the cam is moving onthe pole.
            Vector3 pos = _cam.transform.localPosition;
            float Z = pos.z + ZoomSpeed * translation * _cam.transform.localPosition.magnitude;
            pos.z = Mathf.Clamp(Z, ZoomMax, ZoomMin);
            _cam.transform.localPosition = pos;

            StopAnimation();
        }
        private void CheckSelection(Vector3 selectorPos) {
            Debug.Log("CheckSelection method called."); // This should log when the method is called
            // Ray is casted onto the UI to see if we catched one of the icons
            Ray ray = _cam.ScreenPointToRay(selectorPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, RayLength)) {
                if (hit.collider != null) {
                    // Play sound regardless of whether the selected body has changed
                    AudioManager.Instance.PlayPlanetFocusSound();

                    // Check if the hit object is different from the currently selected body
                    if (_selectedBody != hit.collider.transform) {
                        Debug.Log("Hit object: " + hit.collider.name);
                        _selectedBody = hit.collider.transform;
                        AudioManager.Instance.PlayPlanetFocusSound();
                        NewFocus?.Invoke(_selectedBody); // Use ?.Invoke to safely invoke the event
                        _focus.text = _selectedBody.name;
                    }
                }
            } else {
                Debug.Log("Raycast did not hit any object."); // Add this line for debugging
            }
        }
        }
        
    }
   
    

