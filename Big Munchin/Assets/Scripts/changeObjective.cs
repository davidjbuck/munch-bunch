using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class changeObjective : MonoBehaviour
{
    public GameObject textTrigger;
    public TextMeshProUGUI objectiveTXT;

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.tag);

        if (col.tag == "Player")
        {
            objectiveTXT.text = "New Objective: Chase Tony into the Forest";
            Destroy(textTrigger);
        }
    }
}
