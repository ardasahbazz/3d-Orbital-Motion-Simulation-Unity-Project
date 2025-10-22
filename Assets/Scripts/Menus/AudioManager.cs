using UnityEngine;

namespace solsyssim {
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance; // Singleton instance of AudioManager
        public static AudioManager Instance { get { return instance; } } // Public getter for the singleton instance

        public AudioClip backgroundMusic; // Background music clip
        public AudioClip buttonClickSoundClip; // Sound clip for button clicks
        public AudioClip planetFocusSoundClip; // Sound clip for focusing on a planet
        public AudioClip onSoundClip; // Sound clip for "on" actions
        public AudioClip offSoundClip; // Sound clip for "off" actions
        private AudioSource audioSource; // Audio source for playing music
        private AudioSource sfxSource; // Audio source for playing sound effects

        // Volume controls
        public float masterVolume = 1f; // Master volume level
        public float musicVolume = 1f; // Music volume level
        public float sfxVolume = 1f; // Sound effects volume level

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject); // Ensures only one instance of AudioManager exists
                return;
            }
            else
            {
                instance = this; // Assigns this instance to the static instance variable
            }
            DontDestroyOnLoad(this.gameObject); // Prevents AudioManager from being destroyed on scene load
            LoadVolumeSettings(); // Loads volume settings from PlayerPrefs
            // Initialize sound effects source
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.spatialBlend = 0; // Sets sound to 2D
            // Initialize background music source
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = backgroundMusic; // Assigns the background music clip
            audioSource.loop = true; // Sets the music to loop
            UpdateMusicVolume(); // Applies the current music volume settings
            audioSource.Play(); // Starts playing the background music
        }

        public void SetMasterVolume(float volume)
        {
            masterVolume = volume; // Sets the master volume level
            UpdateVolumeSettings(); // Updates all volume settings
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = volume; // Sets the music volume level
            UpdateMusicVolume(); // Updates the music volume
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = volume; // Sets the SFX volume level
            UpdateVolumeSettings(); // Updates all volume settings
        }

        void UpdateMusicVolume()
        {
            if (audioSource != null)
            {
                audioSource.volume = musicVolume * masterVolume; // Applies the calculated music volume
            }
        }

        void UpdateVolumeSettings()
        {
            // This method would be expanded to update all audio sources' volumes
            UpdateMusicVolume(); // Currently, it only updates the music volume

            SaveVolumeSettings(); // Saves the current volume settings to PlayerPrefs
        }

        void SaveVolumeSettings()
        {
            // Saves volume settings to PlayerPrefs
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            PlayerPrefs.Save(); // Commits changes to PlayerPrefs
        }

        void LoadVolumeSettings()
        {
            // Loads volume settings from PlayerPrefs, with a default value of 1 if not set
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }

        private void PlaySoundEffect(AudioClip clip)
        {
            if (clip != null)
            {
                sfxSource.PlayOneShot(clip, sfxVolume * masterVolume); // Plays a sound effect with the current volume settings
            }
        }

        public void PlayButtonClickSound()
        {
            PlaySoundEffect(buttonClickSoundClip); // Plays the button click sound effect
        }

        public void PlayPlanetFocusSound()
        {
            PlaySoundEffect(planetFocusSoundClip); // Plays the planet focus sound effect
        }

        public void PlayOnSound(){
            PlaySoundEffect(onSoundClip); // Plays the "on" sound effect
        }

        public void PlayOffSound(){
            PlaySoundEffect(offSoundClip); // Plays the "off" sound effect
        }

        public float GetMasterVolume()
        {
            return masterVolume; // Returns the current master volume level
        }

        public float GetMusicVolume()
        {
            return musicVolume; // Returns the current music volume level
        }

        public float GetSFXVolume()
        {
            return sfxVolume; // Returns the current SFX volume level
        }
    }
}
