using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonyInteractable : MonoBehaviour, IInteractable
{
    public GameObject player;
    public GameObject kickScene;
    public void tonyInteraction()
    {
        Debug.Log("TONY INTERACT");
        this.gameObject.SetActive(false);
        kickScene.SetActive(true);
        //player.SetActive(false);
    }
    public void Interact(Transform interactorTransform)
    {
        tonyInteraction();
    }
    public string GetInteractText()
    {
        return "Kick";
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
