using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = true;

    public GameObject mainMenu; //First screen when you hit Escape
    public GameObject pauseMenu; //Current menu being paused
    void Start()
    {
        
    }
    public void PauseGame() //Freezes the game using timeScale and opens the first pause menu
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void BackMenu() //Returns to the prior pause screen
    {
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
        isPaused = true;      
    }

    public void ResumeGame() //Unfreezes time and unpauses the game 
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
        Application.Quit();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused && pauseMenu.activeSelf == true && mainMenu.activeSelf == false) //BackMenu
            {
                BackMenu();
                isPaused = true;
                Debug.Log("Is Paused: " + isPaused);
            }
            else

            if (isPaused && mainMenu.activeSelf == true && pauseMenu.activeSelf == true) //When at the first pause screen, "mainMenu" and "pauseMenu" will be the same object
            {
                ResumeGame();
                Debug.Log("Resuming Game");
            }

            else
            {
                PauseGame();
                Debug.Log("Is Paused: " + isPaused);
            }
        }

    }
}
