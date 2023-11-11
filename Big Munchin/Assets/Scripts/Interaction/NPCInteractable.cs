using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    public GameObject allyText;

    public void Interact(Transform interactorTransform)
    {
        Debug.Log("INTERACT");
        allyText.SetActive(true);
        //ChatBubble3D.Create(transformtransform, new Vector3(-.3f, 1.7f, 0f), ChatBubble3D.IconType.Happy, "Hello There!");
    }
    public string GetInteractText()
    {
        return interactText;
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
