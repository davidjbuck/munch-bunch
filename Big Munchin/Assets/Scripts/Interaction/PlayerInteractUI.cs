using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject allyInteractContainer;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TMP_Text interactTextMeshProUGUI;
    private void Update()
    {
        if(playerInteract.GetInteractableObject() != null)
        {
            Show(playerInteract.GetInteractableObject());
        } else
        {
            Hide();
        }
    }
    private void Show(IInteractable interactable)
    {
        allyInteractContainer.SetActive(true);
        interactTextMeshProUGUI.text = interactable.GetInteractText();
    }
    private void Hide()
    {
        allyInteractContainer.SetActive(false);
    }
}
