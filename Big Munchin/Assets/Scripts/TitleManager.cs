using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

using TMPro;

public class TitleManager : MonoBehaviour
{
    //for in-game
    //public GameObject mButton;
    //public GameObject menuPanel;
    //public float enemySoundVal = 1;

    //for pre-game
    public GameObject buttonsHolder;
    public GameObject optionsHolder;

    //for both i guess
    public AudioSource ambientSound;

    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    int currentRes = 0;

    public TMP_Dropdown shadowsDropdown;
    int shadowVal;

    public Toggle fullscreenToggle;
    bool FST = false;

    public TMP_Dropdown graphicsDropdown;
    int graphics = 0;

    public Slider volSlide;
    float volume = 1;

    public Light mainLight;

    public GameObject creditHolder;

    private void Start()
    {
        //for setting resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resOptionsTxt = new List<string>();
        for (int x = 0; x < resolutions.Length; x++)
        {
            string temp = resolutions[x].width + " x " + resolutions[x].height;
            resOptionsTxt.Add(temp);

            if (resolutions[x].width == Screen.currentResolution.width && resolutions[x].height == Screen.currentResolution.height)
            {
                currentRes = x;
            }
        }
        resolutionDropdown.AddOptions(resOptionsTxt);
        resolutionDropdown.value = currentRes;
        resolutionDropdown.RefreshShownValue();
    }

    //public void menuButton()
    //{
    //    Time.timeScale = 0;
    //    menuPanel.SetActive(true);
    //    mButton.SetActive(false);
    //}
    //public void menuClose()
    //{
    //    Time.timeScale = 1;
    //    menuPanel.SetActive(false);
    //    mButton.SetActive(true);
    //}
    //public void settingsMenu()
    //{
    //    menuPanel.SetActive(false);
    //    settingsPanel.SetActive(true);
    //}
    //public void settingsClose()
    //{
    //    menuPanel.SetActive(true);
    //    settingsPanel.SetActive(false);
    //}

    public void optionsMenu()
    {
        buttonsHolder.SetActive(false);
        optionsHolder.SetActive(true);
    }
    public void unOptionsMenu()
    {
        buttonsHolder.SetActive(true);
        optionsHolder.SetActive(false);
        creditHolder.SetActive(false);
    }
    public void volumeSlider(float v)
    {
        ambientSound.volume = v;
        volume = v;
    }
    public void graphicsDrop(int g)
    {
        QualitySettings.SetQualityLevel(g);
        graphics = g;
    }
    public void fullToggle(bool f)
    {
        Screen.fullScreen = f;
        FST = f;
    }
    public void setResolution(int r)
    {
        Resolution tempRes = resolutions[r];
        Screen.SetResolution(tempRes.width, tempRes.height, Screen.fullScreen);
    }
    public void setShadows(int s)
    {
        if (s == 0)
        {
            mainLight.shadows = LightShadows.Soft;
            shadowVal = 0;
        }
        else if (s == 1)
        {
            mainLight.shadows = LightShadows.Hard;
            shadowVal = 1;
        }
        else
        {
            mainLight.shadows = LightShadows.None;
            shadowVal = 2;
        }
    }
    public void newGame()
    {
        Debug.Log("whoooooooa (scene change here)");
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void openCredits()
    {
        buttonsHolder.SetActive(false);
        creditHolder.SetActive(true);
    }
}
