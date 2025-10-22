using UnityEngine;
using System;

namespace solsyssim {

    // Handles all scales and dimention of space and time for the current gameinstance.
    
    public class SpaceTime : MonoBehaviour {


        private static SpaceTime _instance;
        public static SpaceTime Instance {
            get {
                 if (_instance == null) {
                     if (_instance == null){
                         Debug.LogError("SpaceTime Singleton not yet instanciated.");
                         _instance = new SpaceTime();
                     }
                 }
                return _instance;
            }
        }

        private void Awake() {
            if (_instance != null && _instance != this) {
                Debug.LogWarning("Double instance of SpaceTime Singleton!");
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
        }

        // enum of all the different accessible scales. made it enum as it will be useful for a swtich statement
        public enum Scale{Time, Orbit, Body};
        private float _bodyScale = BaseBodyScale; //defualt scale 

        public float BodyScale {
            get { 
                return _bodyScale;
            }
            private set { 
                //clamps the values the scale can be to a specific range
                _bodyScale = Mathf.Clamp(value, MinDefaultScale, MaxDefaultScale);
                //Updates scale if needed
                if(ScaleUpdated != null)
                    ScaleUpdated.Invoke(Scale.Body, _bodyScale);
            }
        }

        private float _orbitScale = BaseOrbitScale;

        public float OrbitScale {
            get { return _orbitScale; }
            private set { 
                //clamps the values the scale can be to a specific range
                _orbitScale = Mathf.Clamp(value, MinDefaultScale, MaxDefaultScale);
                //Updates scale if needed
                if(ScaleUpdated != null)
                    ScaleUpdated.Invoke(Scale.Orbit, _orbitScale);
            }
        }

        // time scale allows you to pause the game 
        // it also has a totaltime counter and the last scaled deltatime
        //simulation starts with time not paused
        public bool _timePause = false;
        private float _lastTimeScale = BaseTimeScale;
        private float _timeScale = BaseTimeScale;

        public float TimeScale {
            get { return _timeScale; }
            //checks if game is unpaused, if so it clamps the values the scale for the speed of time can be and updates it
            private set { 
                if(!_timePause){
                    _timeScale = Mathf.Clamp(value, MinTimeScale, MaxTimeScale);
                    if(ScaleUpdated != null)
                        ScaleUpdated.Invoke(Scale.Time, _timeScale);
                }
            }
        }

        private double _elapsedTime = 0;
        public double ElapsedTime {
            get { return _elapsedTime; }
            private set { _elapsedTime = value; }
        }

        public float DeltatTime {
            get { return Time.deltaTime * TimeScale; }
        }

        // constant parameters
        private const float BaseBodyScale = 0.01f;
        private const float BaseOrbitScale = 0.0001f;
        private const float BaseTimeScale = 0.5f;
        private const float MinTimeScale = 0.01f;
        private const float MaxTimeScale = 500.0f;
        private const float MinDefaultScale = 0.0001f; // probably replace default by specifics for body and orbits
        private const float MaxDefaultScale = 1.0f;

        // SpaceTime Signals
        public event Action<Scale, float> ScaleUpdated;

        // registering to events
        private void Start() {
            ControlIntentions.Instance.Scaling += UpdateScale;
            ControlIntentions.Instance.PauseGame += PauseTime;        
        }

        private void OnDestroy() {
            ControlIntentions.Instance.Scaling -= UpdateScale;
            ControlIntentions.Instance.PauseGame -= PauseTime;                
        }
        
        // updating the elapsed time each frame depending on frame and scale
        private void Update() {
            ElapsedTime += Time.deltaTime * TimeScale;
        }

        // listening to events from inputs
        // modify the correct scale accordingly
        private void UpdateScale(Scale scale, float value) {

            switch(scale) {
                
                case Scale.Body:
                BodyScale *= 1f + value;
                break;

                case Scale.Orbit:
                OrbitScale *= 1f + value;
                break;

                case Scale.Time:
                TimeScale *= 1f + value;
                break;

                default:
                Debug.LogWarning("Unknown scale when updating scales.");
                break;
            }
        }

        public void TogglePause() {
            bool isPaused = SpaceTime.Instance.TimeScale == 0;
            SpaceTime.Instance.PauseTime(!isPaused);
        }
        // Sets timescale to the minimum when pause is pressed.
        public void PauseTime(bool pause) {
            if(pause){
                _lastTimeScale = TimeScale;
                TimeScale = 0;
                _timePause = true;
            //and resets it back to original when pressed again
            } else {
                _timePause = false;
                TimeScale = _lastTimeScale;
            }
             ScaleUpdated?.Invoke(Scale.Time, _timeScale);

        }

        // Resets the different scales to their default constant values.
        public void ResetAllScales() {
            BodyScale = BaseBodyScale;
            OrbitScale = BaseOrbitScale;
            TimeScale = BaseTimeScale;
        }

    }
}