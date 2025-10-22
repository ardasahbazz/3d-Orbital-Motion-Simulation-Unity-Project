﻿using UnityEngine;
using System;

namespace solsyssim {

    // This script handles all control input and checks the game state it sends a signal to whoever is interested in what inpu has been given by the user.
    public class ControlIntentions : MonoBehaviour {

        // Singleton instantiation
        private static ControlIntentions _instance;
        public static ControlIntentions Instance {
            get {
                // if (_instance == null){
                //     if (_instance == null){
                //         Debug.LogError("Singleton not yet instanciated.");
                //         _instance = new ControlIntentions();
                //     }
                // }
                return _instance;
            }
        }

        // checks the singleton instance
        private void Awake() {
            if (_instance != null && _instance != this) {
                Debug.LogWarning("Double instance of ControlIntentions Singleton!");
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
        }

        // Property to let the programe create fake key press input
        private static String _simulatedInput = "";
        public static String SimulatedInput {
            set {
                _simulatedInput = value;
            }
            get {
                string _key = _simulatedInput;
                _simulatedInput = "";
                return _key;
            }
        }

        // Game/input state var and related events
        public enum State { Game, Menu, Scaling };
        private State _state = State.Menu;

        public event Action<SpaceTime.Scale, float> Scaling;
        private void RaiseScaling(SpaceTime.Scale scale, float value) {
            if(Scaling != null)
                Scaling.Invoke(scale, value);
        }
        public event Action<string, float> CamRotation;
        private void RaiseCamRotation(string axe, float dir) {
            if(CamRotation != null)
                CamRotation.Invoke(axe, dir);
        }
        public event Action<float> CamTranslation;
        private void RaiseCamTranslation(float value) {
            if(CamTranslation != null)
                CamTranslation.Invoke(value);
        }
        public event Action<Vector3> FocusSelection;
        private void RaiseFocusSelection(Vector3 mousePosition) {
            if(FocusSelection != null)
                FocusSelection.Invoke(mousePosition);
        }
        public event Action<bool> MenuCall;
        private void RaiseMenuCall(bool call) {
            if(MenuCall != null)
                MenuCall.Invoke(call);
        }
        public event Action<bool> PauseGame;
        private void RaisePauseGame(bool pause) {
            _gamePaused = pause;
            if(PauseGame != null)
                PauseGame.Invoke(_gamePaused);
        }
        private bool _gamePaused = true;
        private void Update () {
            switch(_state){

                case State.Game:
                    CheckGameInput();
                    break;

                case State.Menu:
                    CheckMenuInput();
                    break;

                case State.Scaling:
                    CheckScalingInput();
                    break;

                default:
                    Debug.LogError("Unknown game state when checking for input.");
                    break;
            }
        }

        private void CheckGameInput() {
            // check condition for changing state
            if (Input.GetKeyDown(KeyCode.Escape) || SimulatedInput == "menu" || Input.GetButtonDown("menu")) {
                _state = State.Menu;
                RaisePauseGame(!_gamePaused);
                RaiseMenuCall(true);
            } else if ((Input.GetButtonDown("scale time") || Input.GetButtonDown("scale orbits") || Input.GetButtonDown("scale bodies"))){
                _state = State.Scaling;
            }
            
            // pause is not a full state as besides time, control should reamin operational
            if (Input.GetButtonDown("pause game"))
                RaisePauseGame(!_gamePaused);// wrote this instwead of true so that s asingle button cna iterate through true and false
            
            // Rotation of the cam around the center horizontally (on the y axis of Axis).
            if (Input.GetAxis("rotate cam horizontally") != 0)
                RaiseCamRotation("horizontal", Input.GetAxis("rotate cam horizontally"));

            // Rotation of the cam around the center vertically (on the x axis of Pole).
            if (Input.GetAxis("rotate cam vertically") != 0)
                RaiseCamRotation("vertical", Input.GetAxis("rotate cam vertically"));
            
            // Translation of the cam on the z axis (from and away)
            if (Input.GetAxis("translate cam (zoom)") != 0)
                RaiseCamTranslation(Input.GetAxis("translate cam (zoom)"));

            // Focus Body Selection
            if (Input.GetMouseButtonDown(0)) {
                //Debug.Log("Attempting to focus on a body at position: " + Input.mousePosition);
                RaiseFocusSelection(Input.mousePosition);
    }
        }
    
        private void CheckMenuInput() {
            // check condition for changing state
            // hardcoded to always be able to access menus and quit
            if (Input.GetKeyDown(KeyCode.Escape) || SimulatedInput == "menu" || Input.GetButtonDown("menu")) {
                _state = State.Game;
                RaiseMenuCall(false);
            }
        }
    
        private void CheckScalingInput() {

            // check condition for changing state
            if (!Input.GetButton("scale time") && !Input.GetButton("scale orbits") && !Input.GetButton("scale bodies"))
                _state = State.Game;
            
            // check user input
            if (Input.GetButton("scale bodies"))
                RaiseScaling(SpaceTime.Scale.Body, Input.GetAxis("scale axis")); //"scale axis" is/should be the "Mouse ScrollWheel"
            else if (Input.GetButton("scale orbits"))
                RaiseScaling(SpaceTime.Scale.Orbit, Input.GetAxis("scale axis"));
            else if (Input.GetButton("scale time"))
                RaiseScaling(SpaceTime.Scale.Time, Input.GetAxis("scale axis"));
        }

    }   
}