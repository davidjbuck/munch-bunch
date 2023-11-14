using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    private bool open;

    public void ToggleDoor()
    {
        Debug.Log("TOGGLE DOOR");
        if (open)
        {
            this.transform.Rotate(0, 90, 0);
            open = false;
        } else if (!open)
        {
            this.transform.Rotate(0, -90, 0);
            open = true;
        }
    }
    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Door INTERACT");
        ToggleDoor();
    }
    public string GetInteractText()
    {
        return "Open/Close Door";
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public void hideText() { }
}
