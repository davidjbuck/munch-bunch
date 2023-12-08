using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnlockCursor();
    }
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // Update is called once per frame
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);

    }
    public void Quit()
    {
        Application.Quit();
    }
    void Update()
    {
        
    }
}
