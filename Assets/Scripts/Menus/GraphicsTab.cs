using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsTab : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fpsDropdown;
    public TMP_Dropdown antiAliasingDropdown;
    public Toggle fullscreenToggle; // Assuming you have a fullscreen toggle
    Resolution[] resolutions;

    void Start()
    {
        InitializeResolutionDropdown();
        InitializeFPSDropdown();
        InitialiseAntiAliasingDropdown();


        // Subscribe to dropdown and toggle changes
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fpsDropdown.onValueChanged.AddListener(SetFPS);
        antiAliasingDropdown.onValueChanged.AddListener(SetAntiAliasing);
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    void InitializeResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
    }

    void InitializeFPSDropdown()
    {
        List<int> fpsOptions = new List<int> { 30, 60, 120, 144, 240 };
        fpsDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (int fps in fpsOptions)
        {
            options.Add(fps.ToString() + " FPS");
        }
        fpsDropdown.AddOptions(options);
    }

    void InitialiseAntiAliasingDropdown()
    {
        List<int> aaOptions = new List<int> { 0, 2, 4, 8 };
        antiAliasingDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (int aa in aaOptions)
        {
            string option = aa > 0 ? aa.ToString() + "x" : "Off";
            options.Add(option);
        }
        antiAliasingDropdown.AddOptions(options);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityIndex", qualityIndex);
    }

    public void SetFPS(int fpsIndex)
    {
        // Assuming fpsDropdown options directly correspond to target frame rates
        int fps = int.Parse(fpsDropdown.options[fpsIndex].text.Split(' ')[0]);
        Application.targetFrameRate = fps;
        PlayerPrefs.SetInt("FPSIndex", fpsIndex);
    }

    public void SetAntiAliasing(int aaIndex)
    {
        // Assuming antiAliasingDropdown options directly correspond to AA values
        int aaValue = int.Parse(antiAliasingDropdown.options[aaIndex].text.Replace("x", "").Replace("Off", "0"));
        QualitySettings.antiAliasing = aaValue;
        PlayerPrefs.SetInt("AAIndex", aaIndex);
    }

    void LoadGraphicsSettings()
    {
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", 0);
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1;

        int qualityIndex = PlayerPrefs.GetInt("QualityIndex", 2);
        QualitySettings.SetQualityLevel(qualityIndex);

        fpsDropdown.value = PlayerPrefs.GetInt("FPSIndex", 1);
        fpsDropdown.RefreshShownValue();

        antiAliasingDropdown.value = PlayerPrefs.GetInt("AAIndex", 0);
        antiAliasingDropdown.RefreshShownValue();
    }
    public void Save(){
        PlayerPrefs.Save();
    }
}