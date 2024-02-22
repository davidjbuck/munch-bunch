using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenu;
    SaveLoad saveLoadScript;
    void Start()
    {
        saveLoadScript = GameObject.FindGameObjectWithTag("save load").GetComponent<SaveLoad>();
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {

    }
  
    public void QuitGame()
    {
        //GameObject save = GameObject.FindGameObjectWithTag("save load");
        saveLoadScript.SaveGame();
        //saveLoadScript.Load();

        Debug.Log("QUIT GAME");
        //Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
                Debug.Log("Is Paused: " + isPaused);
            }

            else
            {
                PauseGame();
                Debug.Log("Is Paused: " + isPaused);
            }
        }
    }
}
