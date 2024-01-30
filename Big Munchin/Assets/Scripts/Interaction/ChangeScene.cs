using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour, IInteractable
{
    public string newScene;
    public string levelName;
    public float transitionTime = 1;
    public Animator transition;
    public void Interact(Transform interactorTransform)
    {
        //Debug.Log("Door INTERACT");
        //ToggleDoor();
        LoadNextLevel();
    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }
    IEnumerator LoadLevel()
    {
        transition.SetTrigger("StartCrossfade");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelName);
    }
    public string GetInteractText()
    {
        
        return "Go to " + newScene + " Scene";
    }
    public Transform GetTransform()
    {
        return transform;
    }
}

