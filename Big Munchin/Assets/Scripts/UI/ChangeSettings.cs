using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
public class ChangeSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;
    void Start()
    {
        QualitySettings.SetQualityLevel(1);

        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for(int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }
        List<string> options = new List<string>();
        for(int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + " x " + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + "Hz";
            options.Add(resolutionOption);
            {
                if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Volume Changers
    public void ChangeMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVol", volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVol", volume);
    }

    public void ChangeSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVol", volume);
    }

    public void ChangeVoiceVolume(float volume)
    {
        audioMixer.SetFloat("VoicesVol", volume);       
    }

    //----------------------------------------------------------------

    //Graphics Changers
    public void ChangeGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void ChangeShadows(int shadowIndex)
    {
        if (shadowIndex == 0)
        {
            QualitySettings.shadows = ShadowQuality.Disable;
            Debug.Log(shadowIndex);
        }

        if (shadowIndex == 1)
        {
            QualitySettings.shadows = ShadowQuality.HardOnly;
            Debug.Log(shadowIndex);
        }

        if (shadowIndex == 2)
        {
            QualitySettings.shadows = ShadowQuality.All;
            Debug.Log(shadowIndex);
        }
    }

    public void ChangeResolutions(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }
    public void ToggleFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("Full screen :)");
    }
}
