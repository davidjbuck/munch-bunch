using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject allyInteractContainer;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TMP_Text interactTextMeshProUGUI;
    private int counter;
    private void Update()
    {
        if(playerInteract.GetInteractableObject() != null)
        {
            Show(playerInteract.GetInteractableObject());
            counter = 0;
        } else if (counter < 100)
        {
            counter++;
        }
        if(counter >= 100)
        {
            Hide();
        }
    }
    private void Show(IInteractable interactable)
    {
        allyInteractContainer.SetActive(true);
        interactTextMeshProUGUI.text = interactable.GetInteractText();
    }
    public void Hide()
    {
        allyInteractContainer.SetActive(false);

    }
}
