using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NGcontroller : MonoBehaviour
{
    int currentScreen = 0;

    //gets the name of the selected button and instantiates that object
    public void createObject()
    {
        string foodItem = EventSystem.current.currentSelectedGameObject.name;
        //instantiate object from name
    }

    public void openRecipeBook()
    {
        //onclick, activate ui object for recipe book
    }
}
