using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class harvestInteraction : MonoBehaviour
{
    public string hName;
    bool hasEntered = false;
    public TextMeshProUGUI harvestTXT;

    public GameObject MC;

    private void Update()
    {
        if (hasEntered)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                int tempMissNumb = MC.GetComponent<missionController>().getCurrentMission();
                if (tempMissNumb == 0)
                {
                    tempMissNumb++;
                    MC.GetComponent<missionController>().setCurrentMission(tempMissNumb);
                    Debug.Log("entered getE, tempmissnumb: " + tempMissNumb);
                }

                int amountInstant = Random.Range(1, 4);
                for (int x = 0; x < amountInstant; x++)
                {
                    Instantiate(Resources.Load(hName), transform.position, Quaternion.identity);
                }
                harvestTXT.text = "";
                Destroy(gameObject);
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
