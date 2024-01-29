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

                Instantiate(Resources.Load(hName), transform.position, Quaternion.identity);
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        hasEntered = true;
        harvestTXT.text = "Press E to Harvest " + hName;
    }
}
