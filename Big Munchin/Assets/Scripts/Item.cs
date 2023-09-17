using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //item variables
    public string itemName;
    public string flavorText;
    public int amount;
    public int itemType;


    //destroys the object on contact
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
