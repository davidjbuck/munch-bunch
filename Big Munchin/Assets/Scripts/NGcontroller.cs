using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NGcontroller : MonoBehaviour
{
    int currentScreen = 0;
    public GameObject knife;
    public GameObject counter;
    string currentMade;

    //gets the name of the selected button and instantiates that object
    public void createObject()
    {
        string foodItem = EventSystem.current.currentSelectedGameObject.name;
        if(foodItem != currentMade)
        {
            Instantiate(Resources.Load(foodItem), new Vector3(4.105f, 1.181f, 182.282f), Quaternion.identity);
            counter.GetComponent<sliceObject>().getSliceObject(foodItem);
            knife.SetActive(true);
            currentMade = foodItem;
        }
    }

    public void openRecipeBook()
    {
        //onclick, activate ui object for recipe book
    }
}
