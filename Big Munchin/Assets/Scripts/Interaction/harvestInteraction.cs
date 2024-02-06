using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class harvestInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;

    private bool interacted;
    public GameObject harvestText;
    public GameObject Player;


    public void Interact(Transform interactorTransform)
    {
        interacted = true;
        showText();
    }
    public void showText()
    {
        harvestText.SetActive(true);
    }
    public void hideText()
    {
        harvestText.SetActive(false);
    }
    public string GetInteractText()
    {
        return interactText;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public void Update()
    {
        if (interacted)
        {
            float dist = Vector3.Distance(this.GetTransform().position, Player.transform.position);
            Vector3 playerPosition = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
            if (dist > 5.5f)
            {
                hideText();
                interacted = false;
            }
        }
    }
}
