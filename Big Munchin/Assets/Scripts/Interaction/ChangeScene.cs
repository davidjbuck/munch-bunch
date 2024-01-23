using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour, IInteractable
{
    public string newScene;
    public string levelName;
    public void Interact(Transform interactorTransform)
    {
        //Debug.Log("Door INTERACT");
        //ToggleDoor();
        SceneManager.LoadScene(levelName);
    }
    public string GetInteractText()
    {
        return newScene;
    }
    public Transform GetTransform()
    {
        return transform;
    }
}

