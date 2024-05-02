using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTony : MonoBehaviour
{ 
    public GameObject Tony;

    public GameObject mc;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("SPAWN TONY");
            Tony.SetActive(true);

            mc.GetComponent<missionController>().setCurrentMission(10);
        }
    }
}
