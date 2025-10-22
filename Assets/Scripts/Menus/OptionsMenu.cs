using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace solsyssim{

    public class OptionsMenu : MonoBehaviour
    {
        
        
        // References to sliders
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        
        private void Start(){
            // Load saved volume settings or use default values
            _masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", AudioManager.Instance.GetMasterVolume());
            _musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", AudioManager.Instance.GetMusicVolume());
            _sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", AudioManager.Instance.GetSFXVolume());

            // Initialize sliders and add listeners
            _masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            _musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            _sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        public void SetMasterVolume(float volume) {
            AudioManager.Instance.SetMasterVolume(volume);
            PlayerPrefs.SetFloat("MasterVolume", volume);
            PlayerPrefs.Save(); // Save changes immediately
        }

        public void SetMusicVolume(float volume) {
            AudioManager.Instance.SetMusicVolume(volume);
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save(); // Save changes immediately
        }

        public void SetSFXVolume(float volume) {
            AudioManager.Instance.SetSFXVolume(volume);
            PlayerPrefs.SetFloat("SFXVolume", volume);
            PlayerPrefs.Save(); // Save changes immediately
        }


        public void PlaySolarSystem(){
        SceneManager.LoadScene("StellarSystem"); 
        PlayerPrefs.Save();
        }
        public void QuitGame(){ 
        Application.Quit(); 
        PlayerPrefs.Save();
        }
        public void PlayMainMenu(){
        SceneManager.LoadScene("MainMenu"); 
        PlayerPrefs.Save();
        }
        public void Save(){
            PlayerPrefs.Save();
        }
    }
}