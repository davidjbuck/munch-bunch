using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //creates the list for the inventory
    public List<Item> inventoryList = new List<Item>();
    
    //creates the list for the button text
    private List<TextMeshProUGUI> buttonList = new List<TextMeshProUGUI>();

    //the object that stores all inventory UI, and bool for if it is active or not
    private GameObject inventoryParent;
    private bool inventoryOpen = true;

    //the text objects for the inventory display text
    private TextMeshProUGUI ITDName;
    private TextMeshProUGUI ITDType;
    private TextMeshProUGUI ITDFlavorText;

    //string for selected item
    string selectedItem;


    public missionController mc;

    //goes with the cursed mission function
    private bool playerCrossedFarm = false;
    private bool playerCrossedTony = false;
    private void Start()
    {
        inventoryParent = GameObject.Find("inventoryParent");
        buttonList = new List<TextMeshProUGUI>();
        TextMeshProUGUI[] temp = inventoryParent.GetComponentsInChildren<TextMeshProUGUI>();
        for(int i = 0;i < 10; i++)
        {
            buttonList.Add(temp[i]);
        }
        ITDName = temp[10];
        ITDType = temp[11];
        ITDFlavorText = temp[12];

        //sets all button texts to null
        foreach (TextMeshProUGUI tmpu in buttonList)
        {
            tmpu.text = "";
        }
        inventoryOpen = false;
        inventoryParent.SetActive(inventoryOpen);
    }


    private void Update()
    {
        //on hitting inventory button ("i"), fills the button texts with items,
        //activates UI object, and switches the toggle
        if (Input.GetKeyDown(KeyCode.I))
        {
            fillGUIButtons();            
            inventoryOpen = !inventoryOpen;
            inventoryParent.SetActive(inventoryOpen);
        }
    }

    //this is an incredibly cursed way of activating the mission for the goons in the chicken farm
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bowlOnScale" && !playerCrossedFarm && mc.getCurrentMission() == 6)
        {
            Debug.Log("box for trigger entered");
            playerCrossedFarm = true;
            mc.setCurrentMission(7);
        }
        else if (other.tag == "bowlOnScale" && !playerCrossedTony && mc.getCurrentMission() == 11)
        {
            Debug.Log("box for trigger entered");
            playerCrossedTony = true;
            mc.setCurrentMission(12);
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        //makes sure the collided object is an item
        if (collision.gameObject.tag == "item")
        {
            //gets the item info from the object
            Item tempItem = collision.gameObject.GetComponent<Item>().Clone();

            if (tempItem.itemName == "Broccoli")
            {
                Debug.Log("we made it to the inventory");
                mc.sMissionZeroFunction();
            }
            else if (tempItem.itemName == "Potato Bird")
            {
                Debug.Log("we made it to the inventory BIRDS");
                mc.missionFourFunction();
            }




            //if the item is found already in the list, add another one
            bool itemFound = false;
            foreach (Item item in inventoryList)
            {
                if (tempItem.itemName == item.itemName)
                {
                    item.amount += tempItem.amount;
                    itemFound = true;
                }
            }

            //otherwise add it to the list
            if (!itemFound)
            {
                inventoryList.Add(tempItem);
            }
            fillGUIButtons();
        }
    }

    public void addItem(Item item)
    {
        Debug.Log("Item added");
        inventoryList.Add(item);
    }

    //for now takes in item name, can also change to take in an item parameter
    private void removeItem(string s)
    {
        foreach (Item item in inventoryList)
        {
            if (item.itemName == s)
            {
                inventoryList.Remove(item);
            }
        }
    }


    //sorts the list by the item type, then reprogpogates the list
    public void sortList()
    {
        inventoryList.Sort((x, y) => x.itemType.CompareTo(y.itemType));
        fillGUIButtons();
    }


    //fills text for the buttons with inventory items
    public void fillGUIButtons()
    {
        for (int x = 0; x < buttonList.Count; x++)
        {
            if (x < inventoryList.Count)
            {
                buttonList[x].text = inventoryList[x].itemName + " (" + inventoryList[x].amount + ")";
            }
            else
            {
                buttonList[x].text = "";
            }
            
        }
    }


    //fills the item display text from the currently selected item for the inventory UI
    public void displayText()
    {
        Item tempItem = null;
        foreach (Item item in inventoryList)
        {
            if (item.itemName == selectedItem)
            {
                tempItem = item;
            }
        }
        if(tempItem != null)
        {
            ITDName.text = tempItem.itemName;
            ITDType.text = tempItem.itemType.ToString();
            ITDFlavorText.text = tempItem.flavorText;
        }
        else
        {
            ITDName.text = "";
            ITDType.text = "";
            ITDFlavorText.text = "";
        }
        

        selectedItem = "";
    }


    //gets the text from the currently selected item
    public void currentlySelected()
    {
        string tempString = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text;
        foreach (char a in tempString)
        {
            if (a == ' ')
            {
                break;
            }
            else
            {
                selectedItem += a;
            }
        }
    }

    public Item UseItem(int index)
    {
        int i = 0;
        foreach (Item item in inventoryList)
        {
            if (i == index)
            {
                Item temp = item.Clone();
                item.amount --;
                if(item.amount == 0)
                {

                    inventoryList.Remove(item);
                }                
                return temp;
            }
            else
            {
                i++;
            }
        }
        return inventoryList[0];
    }
    public void LoadItem(Item loadedItem, int numLoaded)
    {
        inventoryList.Add(loadedItem);
        int invListLength = inventoryList.Count;
        inventoryList[invListLength-1].amount = numLoaded;
    }
    public int GetInventoryCapacity()
    {
        return inventoryList.Count;
    }

}
