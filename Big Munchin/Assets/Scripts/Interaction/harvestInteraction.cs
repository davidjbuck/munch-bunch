using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class harvestInteraction : MonoBehaviour
{
    public string hName;
    bool hasEntered = false;
    public TextMeshProUGUI harvestTXT;

    private void Update()
    {
        if (hasEntered)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                harvestTXT.text = "Harvested 1 " + hName;
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        hasEntered = true;
        harvestTXT.text = "Press E to Harvest " + hName;
    }
}
