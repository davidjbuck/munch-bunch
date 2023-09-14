using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public string flavorText;
    public int amount;


    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(gameObject);
    }
}
