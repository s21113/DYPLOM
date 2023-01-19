using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.Rendering;
using UnityEngine.UI;

public enum SettingTab
{
    Gameplay,
    Audio,
    Video,
    Controls
}

public enum QualityMode
{
    Low,
    Medium,
    High,
    Ultra,
    RTMedium,
    RTMax
}


[Serializable]
public class Settings
{
    public static float maxVolume = 100f;
    public int masterVolume = 100, musicVolume = 100, ambianceVolume = 100, fxVolume = 100;
    public bool invertY = false;
    public int fov = 60;
    public float mouseSens = 3f;
    public Resolution resolution;
    public QualityMode quality;
    public bool isFullscreen = true;
    public FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    public float gamma = 1f;
    public bool sprintToggle = false, crouchToggle = true;

    public override string ToString()
    {
        string returner = $"masterVolume: {masterVolume}\n";
        returner += $"musicVolume: {musicVolume}\n";
        returner += $"ambianceVolume: {ambianceVolume}\n";
        returner += $"fxVolume: {fxVolume}\n";
        returner += $"invertY: {invertY}\n";
        returner += $"fov: {fov}\n";
        returner += $"mouseSens: {mouseSens}\n";
        returner += $"resolution: {resolution}\n";
        returner += $"quality: {quality}\n";
        returner += $"isFullscreen: {isFullscreen}\n";
        returner += $"fullScreenMode: {fullScreenMode}\n";
        returner += $"sprintToggle: {sprintToggle}\n";
        returner += $"crouchToggle: {crouchToggle}\n";
        return returner.Trim();
    }
}







public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;

    // Załaduję domyślne ustawienia lol
    private Settings settings = new Settings();

    public AudioMixer audioMixer;

    [Header("Audio Settings")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider ambianceSlider;
    [SerializeField] private Slider effectsSlider;

    [Header("Gameplay Settings")]
    [SerializeField] private Toggle yInvertToggle;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private Slider sensSlider;

    private Resolution[] Video_Resolutions;
    private int currentResolution;
    [Header("Video Settings")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Dropdown fullscreensDropdown;
    [SerializeField] private Slider gammaSlider;

    [Header("Controls Settings")]
    [SerializeField] private Toggle toggleSprintUI;
    [SerializeField] private Toggle toggleCrouchUI;

    [SerializeField]
    private GameObject[] tabs;

    [SerializeField]
    private SettingTab currentTab;

    public static Button backButton;

    private void AdjustChangers(Settings newSettings)
    {
        if (settings.ambianceVolume != newSettings.ambianceVolume)
            ambianceSlider.value = newSettings.ambianceVolume;
        if (settings.fxVolume != newSettings.fxVolume)
            effectsSlider.value = newSettings.fxVolume;
        if (settings.musicVolume != newSettings.musicVolume)
            musicSlider.value = newSettings.musicVolume;
        if (settings.masterVolume != newSettings.masterVolume)
            masterSlider.value = newSettings.masterVolume;

        if (settings.invertY != newSettings.invertY)
            yInvertToggle.isOn = newSettings.invertY;
        if (settings.fov != newSettings.fov)
            fovSlider.value = newSettings.fov;
        if (settings.mouseSens != newSettings.mouseSens)
            sensSlider.value = newSettings.mouseSens;

        if (!settings.resolution.Equals(newSettings.resolution))
        {
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                if (Screen.resolutions[i].Equals(newSettings.resolution))
                {
                    currentResolution = i;
                    break;
                }
            }
        }
        if (settings.isFullscreen != newSettings.isFullscreen)
            fullScreenToggle.isOn = newSettings.isFullscreen;
        if (settings.fullScreenMode != newSettings.fullScreenMode)
            fullscreensDropdown.value = (int)newSettings.fullScreenMode;

        if (settings.crouchToggle != newSettings.crouchToggle)
            toggleCrouchUI.isOn = newSettings.crouchToggle;
        if (settings.sprintToggle != newSettings.sprintToggle)
            toggleSprintUI.isOn = newSettings.sprintToggle;
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        ReadSettingsFromFile();
        ReadSettingsInternally();
        settings.resolution = Screen.currentResolution;
    }

    private void Start()
    {
        Video_Resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        List<string> qualities = new List<string>();
        List<string> fsModes = new List<string>();
        resolutionDropdown.ClearOptions();
        fullscreensDropdown.ClearOptions();
        for (int i = 0; i < Video_Resolutions.Length; i++)
        {
            int w = Video_Resolutions[i].width, h = Video_Resolutions[i].height;
            int rr = Video_Resolutions[i].refreshRate;
            string optionName = $"{w} x {h} @ {rr} Hz";
            options.Add(optionName);
            if (w == Screen.currentResolution.width &&
                h == Screen.currentResolution.height &&
                rr == Screen.currentResolution.refreshRate)
            {
                currentResolution = i;
            }
        }
        foreach (QualityMode mode in Enum.GetValues(typeof(QualityMode)))
            qualities.Add(mode.ToString());
        foreach (FullScreenMode mode in Enum.GetValues(typeof(FullScreenMode)))
            fsModes.Add(mode.ToString());
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.AddOptions(qualities);
        qualityDropdown.RefreshShownValue();
        fullscreensDropdown.AddOptions(fsModes);
        fullscreensDropdown.RefreshShownValue();

        SetTab(SettingTab.Gameplay);
    }

    private void SetTab(SettingTab newTab)
    {
        currentTab = newTab;
        foreach (GameObject tab in tabs)
        {
            if (tab.name.ToLower().Contains(currentTab.ToString().ToLower()))
                tab.SetActive(true);
            else
                tab.SetActive(false);
        }
    }

    public void SetTab(string newTab)
    {
        var vals = Enum.GetValues(typeof(SettingTab));
        foreach (SettingTab val in vals)
        {
            if (newTab.ToLower().Contains(val.ToString().ToLower()))
            {
                SetTab(val);
                return;
            }
        }
    }

    #region GAMEPLAY
    public void SetInvertY(bool newValue)
    {
        settings.invertY = newValue;
    }
    public void SetFOV(float newValue)
    {
        settings.fov = (int)newValue;
    }
    public void SetMouseSensitivity(float newValue)
    {
        settings.mouseSens = newValue;
    }
    #endregion

    #region AUDIO
    public void SetMasterVolume(float newValue)
    {
        settings.masterVolume = (int)newValue;
        AudioListener.volume = newValue / 100f;
    }

    public void SetMusicVolume(float newValue)
    {
        settings.musicVolume = (int)newValue;
        audioMixer.SetFloat(AudioManager.BGM_VOL, AudioManager.A((int)newValue));
    }

    public void SetAmbianceVolume(float newValue)
    {
        settings.ambianceVolume = (int)newValue;
        audioMixer.SetFloat(AudioManager.AMB_VOL, AudioManager.A((int)newValue));
    }

    public void SetEffectsVolume(float newValue)
    {
        settings.fxVolume = (int)newValue;
        audioMixer.SetFloat(AudioManager.BGM_VOL, AudioManager.A((int)newValue));
    }
    #endregion

    #region VIDEO
    public void SetResolution(int newValue)
    {
        Resolution newRes = Screen.resolutions[newValue];
        Screen.SetResolution(newRes.width, newRes.height, settings.fullScreenMode, newRes.refreshRate);
        return;
    }

    public void SetQualityMode(int newValue)
    {
        settings.quality = (QualityMode)newValue;
        QualitySettings.SetQualityLevel(newValue);
        return;
    }

    public void SetFullscreen(bool newValue)
    {
        settings.isFullscreen = newValue;
        Screen.fullScreen = newValue;
        return;
    }

    public void SetFullscreenMode(int newValue)
    {
        settings.fullScreenMode = (FullScreenMode)newValue;
        Screen.fullScreenMode = (FullScreenMode)newValue;
        return;
    }
    #endregion

    #region CONTROLS
    public void SetToggleSprint(bool newValue)
    {
        settings.sprintToggle = newValue;
    }
    public void SetToggleCrouch(bool newValue)
    {
        settings.crouchToggle = newValue;
    }
    #endregion



    #region SERIALIZING
    public Settings GetSettings()
    {
        return settings;
    }
    public void SaveSettingsToFile()
    {
        Settings toSave = new Settings();
        toSave.ambianceVolume = (int)ambianceSlider.value;
        toSave.fxVolume = (int)effectsSlider.value;
        toSave.masterVolume = (int)masterSlider.value;
        toSave.musicVolume = (int)musicSlider.value;
        toSave.invertY = yInvertToggle.isOn;
        toSave.fov = (int)fovSlider.value;
        toSave.mouseSens = sensSlider.value;
        toSave.resolution = Screen.resolutions[currentResolution];
        toSave.quality = (QualityMode)qualityDropdown.value;
        toSave.isFullscreen = fullScreenToggle.isOn;
        toSave.fullScreenMode = (FullScreenMode)fullscreensDropdown.value;
        toSave.crouchToggle = toggleCrouchUI.isOn;
        toSave.sprintToggle = toggleSprintUI.isOn;

        string json = JsonUtility.ToJson(toSave);
        PlayerPrefs.SetString("Settings", json);
        PlayerPrefs.Save();
    }

    private void ReadSettingsInternally()
    {
        Settings newSettings = ReadSettingsFromFile();
        AdjustChangers(newSettings);
        this.settings = newSettings;
    }

    public static Settings ReadSettingsFromFile()
    {
        string json = PlayerPrefs.GetString("Settings");
        Settings settings = JsonUtility.FromJson<Settings>(json);
        if (settings == null || json == "") settings = new Settings();
        settings.resolution = Screen.currentResolution;
        return settings;
    }

    public static void UpdateSettings(out Settings settings)
    {
        settings = new Settings();
        string json = PlayerPrefs.GetString("Settings", "");
        if (json == "") return;
        Settings newSettings = JsonUtility.FromJson<Settings>(json);
        if (newSettings == null) return;
        settings = newSettings;
    }
    #endregion
}
