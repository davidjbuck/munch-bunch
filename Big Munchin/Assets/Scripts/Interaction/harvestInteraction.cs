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
                int amountInstant = Random.Range(1, 4);
                Debug.Log("random: " + amountInstant);
                for (int x = 0; x < amountInstant; x++)
                {
                    Instantiate(Resources.Load(hName), transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
                harvestTXT.text = "";
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        hasEntered = true;
        harvestTXT.text = "Press E to Harvest " + hName;
    }
    public void OnCollisionExit(Collision collision)
    {
        hasEntered = false;
        harvestTXT.text = "";
    }
}
