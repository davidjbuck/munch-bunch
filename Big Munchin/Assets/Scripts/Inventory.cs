using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public List<Item> inventoryList = new List<Item>();
    Item tempItem;


    private void OnCollisionEnter(Collision collision)
    {
        //if the collided object is an item, add it to the inventory
        if (collision.gameObject.tag == "item")
        {
            tempItem = collision.gameObject.GetComponent<Item>();
            addToInventory(tempItem);
            displayInventory();
        }
        else
        {
            //do not
        }
    }

    private void addToInventory(Item i)
    {
        bool addToList = true;
        foreach(Item item in inventoryList)
        {
            if(item.name == i.name)
            {
                item.amount++;
                addToList = false;
            }
        }
        if(addToList)
        {
            inventoryList.Add(tempItem);
        }
    }

    private void displayInventory()
    {
        foreach (Item item in inventoryList)
        {
            //Debug.Log(item.name);
        }
    }
}
