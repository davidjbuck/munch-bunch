using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTony : MonoBehaviour
{ 
    public GameObject Tony;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("SPAWN TONY");
            Tony.SetActive(true);

        }
    }
}
