using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //Declared as reference to handle keyboard input for buttons in the unity editor
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame to contionously check for any user inputs
    void Update()
    {   //Ensures action is only executed once per key press. So if the user held the key down it would only execute once instead of for eveyr frame
        if (Keyboard.current.enterKey.wasPressedThisFrame && button != null && button.interactable)
        {
            // Triggers the button click when Enter key is pressed
            button.onClick.Invoke();
        }
    }

    //keeps a specific button selected
    public void SetFocus()
    {
        button.Select();
    }

    //references to variables so that they can be assigned values to in the unity editor
    public GameObject startPopUp, exitPopUp, aboutPopUp;
    public Selectable exitPopUpFirstButton, startFirstButton, startPopUpFirstButton, aboutPopUpFirstButton, startButton, tutorialButton, optionsButton, exitButton, aboutButton;

    // Opens the start pop-up and sets focus to the first button inside
    public void OpenStartPopUp()
    {
        startPopUp.gameObject.SetActive(true);
        startPopUpFirstButton.Select();
    }

    // Opens the exit pop-up and sets focus to the first button inside
    public void OpenExitPopUp()
    {
        exitPopUp.gameObject.SetActive(true);
        exitPopUpFirstButton.Select();
    }

    // Opens the about pop-up and sets focus to the first button inside
    public void OpenAboutPopUp()
    {
        aboutPopUp.gameObject.SetActive(true);
        aboutPopUpFirstButton.Select();
    }

    // Closes the start pop up and set the focus to the start button in the main menu
    public void CloseStartPopUp()
    {
        startPopUp.gameObject.SetActive(false);
        startButton.Select();
    }

    // Closes the exit pop-up and sets focus to the exit button in the main menu
    public void CloseExitPopUp()
    {
        exitPopUp.gameObject.SetActive(false);
        exitButton.Select();
    }

    // Closes the about pop-up and sets focus to the about button in the main menu
    public void CloseAboutPopUp()
    {
        aboutPopUp.gameObject.SetActive(false);
        aboutButton.Select();
    }

    //Declared as reference to handle keyboard input for buttons in the unity editor
    public void PlaySolarSystem(){
        SceneManager.LoadScene("StellarSystem"); 
    }
    //This function is called by a button to load the scene of an interface to create a custom system
    public void PlayCustomSystem(){
        SceneManager.LoadScene("CustomSystem"); 
    }
    //Called to quir the game
    public void QuitGame(){ 
        Application.Quit(); 
    }
     //Called to load the options menu on the screen
    public void OptionsMenu(){
        SceneManager.LoadScene("OptionsMenu");
    }
}
