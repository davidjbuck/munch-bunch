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
    public int[] itemBuffTypes;//when adding a new item, fill which buff types it has
    //0 = crit chance up, 1 = max stamina up, 2 = stamina regen up, 3 = stamina cost decrease, 
    //4 = player speed up, 5 = instant healing, 6 = healing per second
    public float[] effectDuration;
    public float[] effectValue;


    //destroys the object on contact
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    public Item Clone()
    {
        return (Item)MemberwiseClone();
    }
}
