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

    public GameObject sliceButtonHolder;
    public GameObject slicePlaneHolder;

    public GameObject meatParent;
    public GameObject carbParent;
    public GameObject vegParent;

    //gets the name of the selected button and instantiates that object
    public void createObject()
    {
        string foodItem = EventSystem.current.currentSelectedGameObject.name;
        if(foodItem != currentMade)
        {
            Instantiate(Resources.Load(foodItem), new Vector3(4.105f, 1.181f, 182.282f), Quaternion.identity);
            counter.GetComponent<sliceObject>().getSliceObject(foodItem);
            knife.SetActive(true);
            sliceButtonHolder.SetActive(true);
            slicePlaneHolder.SetActive(true);
            currentMade = foodItem;
        }
    }

    public void switchToCarb()
    {
        currentScreen = 1;
        meatParent.SetActive(false);
        carbParent.SetActive(true);
    }

    public void openRecipeBook()
    {
        //onclick, activate ui object for recipe book
    }
}
