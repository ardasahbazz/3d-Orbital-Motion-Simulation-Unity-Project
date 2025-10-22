using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Used for loading and managing scenes
using UnityEngine.EventSystems; // Provides interfaces and classes for handling events
using UnityEngine.InputSystem; // Input system for handling player input
using UnityEngine.UI; // Namespace for working with UI elements

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    // This method is used to load the options menu scene
    public void PlayOptionsMenu(){
        SceneManager.LoadScene("OptionsMenu"); // Loads the scene named "OptionsMenu"
    }
    // This method is used to quit the game application
    public void QuitGame(){ 
    Application.Quit(); // Quits the game application
    }
    // This method is used to load the main menu scene
    public void PlayMainMenu(){
    SceneManager.LoadScene("MainMenu"); // Loads the scene named "MainMenu"
    }
}
