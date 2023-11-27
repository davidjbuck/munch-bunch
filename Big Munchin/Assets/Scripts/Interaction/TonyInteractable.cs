using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonyInteractable : MonoBehaviour, IInteractable
{
    public void tonyInteraction()
    {
        Debug.Log("TONY INTERACT");

    }
    public void Interact(Transform interactorTransform)
    {
        tonyInteraction();
    }
    public string GetInteractText()
    {
        return "Tony";
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
