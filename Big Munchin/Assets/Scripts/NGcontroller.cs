using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public GameObject riceParent;
    public ParticleSystem ricePour;
    float fakeWeightCount = 0;
    public TextMeshProUGUI weightTxt;

    private void Update()
    {
        //particle system timing
        if (Input.GetMouseButton(0))
        {
            if(riceParent.activeInHierarchy == true)
            {
                fakeWeightCount += Time.deltaTime*2;
                weightTxt.text = ("Weight: " + fakeWeightCount + "lbs");
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            ricePour.Play();
        }
        if (Input.GetMouseButtonUp(0))
        {
            ricePour.Stop();
        }
    }

    //gets the name of the selected button and instantiates that object
    public void createObjectMeat()
    {
        string foodItem = EventSystem.current.currentSelectedGameObject.name;
        if(foodItem != currentMade)
        {
            Instantiate(Resources.Load(foodItem), new Vector3(-392.68f, 28.9f, 60.76f), Quaternion.identity);
            counter.GetComponent<sliceObject>().getSliceObject(foodItem);
            knife.SetActive(true);
            sliceButtonHolder.SetActive(true);
            slicePlaneHolder.SetActive(true);
            currentMade = foodItem;
        }
    }

    public void carbActivate()
    {
        riceParent.SetActive(true);
    }

    public void switchToCarb()
    {
        currentScreen = 1;
        Destroy(GameObject.Find("Upper_Hull"));
        Destroy(GameObject.Find("Lower_Hull"));
        meatParent.SetActive(false);
        carbParent.SetActive(true);
    }
    public void switchToVeg()
    {
        currentScreen = 2;
        riceParent.SetActive(false);
        carbParent.SetActive(false);
        vegParent.SetActive(true);
    }

    public void openRecipeBook()
    {
        //onclick, activate ui object for recipe book
    }
}
