using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NGcontroller : MonoBehaviour
{
    //stores the int for the current screen, the object holding this script, and the current object
    int currentScreen = 0;
    public GameObject counter;
    string currentMade;

    //holds the variables for the calories and weights of each object (and total)
    float meatWeight;
    float carbWeight;
    float vegWeight;
    float totalWeight;
    float meatCal;
    float carbCal;
    float vegCal;
    float totalCal;

    //objects for each screen parent
    public GameObject meatParent;
    public GameObject carbParent;
    public GameObject vegParent;
    public GameObject sumParent;

    //objects for the meat screen
    public GameObject knife;
    public GameObject sliceButtonHolder;
    public GameObject slicePlaneHolder;

    //objects for the carb screen (and weight count tracker)
    public GameObject riceParent;
    public ParticleSystem ricePour;
    float fakeWeightCount = 0;
    public TextMeshProUGUI weightTxt;
    public GameObject scaleObject;

    //objects for the veg screen
    string vegName;
    public TextMeshProUGUI mostPlateWeightTxt;
    public TextMeshProUGUI vegWeightTxt;

    //objects for summary screen
    public TextMeshProUGUI meatSumTXT;
    public TextMeshProUGUI carbSumTXT;
    public TextMeshProUGUI vegSumTXT;
    public TextMeshProUGUI calSumTXT;

    private void Update()
    {
        //if we're on the carb screen
        if(currentScreen == 1)
        {
            //particle system timing
            if (Input.GetMouseButton(0))
            {
                //if the particle system is active (redundant), updates the weight and displays it on scale
                if (riceParent.activeInHierarchy == true)
                {
                    fakeWeightCount += Time.deltaTime * 2;
                    weightTxt.text = (fakeWeightCount.ToString("F2"));
                }
            }

            //plays and stops the rice pouring PS, only if on the carb screen
            if (Input.GetMouseButtonDown(0))
            {
                ricePour.Play();
            }
            if (Input.GetMouseButtonUp(0))
            {
                ricePour.Stop();
            }
        }

        //if on the vegetable screen, creates the vegetable object on click
        //updates and displays the vegetable weight
        else if (currentScreen == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(vegParent != null)
                {
                    Instantiate(Resources.Load(vegName), new Vector3(-397.18f, 31.625f, 60.49f), Quaternion.identity);
                    vegWeight++;
                    vegWeightTxt.text = "Vegetable Weight: " + vegWeight;
                }
            }
        }

        //on summary screen, destroys all created objects
        if (currentScreen == 3)
        {
            Destroy(GameObject.Find("brocoli(Clone)"));
        }
    }

    //gets the name of the selected button and instantiates that object
    public void createObjectMeat()
    {
        //gets string of current button and compares it against "currentMade" to make sure its not the same
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

    //get/set for meat variables from ingredientNMG object
    public void setMeatCounts(float w, float c)
    {
        meatWeight = w;
        meatCal = c;
    }

    //activates the PS from the rice button
    public void carbActivate()
    {
        riceParent.SetActive(true);
    }

    //switches screens from meat to carb (and activates/deactivates respective objects)
    public void switchToCarb()
    {
        currentScreen = 1;
        Destroy(GameObject.Find("Upper_Hull"));
        Destroy(GameObject.Find("Lower_Hull"));
        scaleObject.SetActive(true);
        meatParent.SetActive(false);
        carbParent.SetActive(true);
    }

    //switches screens from carb to veg (and activates/deactivates respective objects)
    public void switchToVeg()
    {
        carbWeight = fakeWeightCount;
        carbCal = fakeWeightCount * 2;
        currentScreen = 2;
        mostPlateWeightTxt.text = "Current Weight: " + (meatWeight + carbWeight);
        riceParent.SetActive(false);
        carbParent.SetActive(false);
        vegParent.SetActive(true);
    }

    //switches screens from veg to sum (and activates/deactivates respective objects)
    public void switchToSummary()
    {
        currentScreen = 3;
        scaleObject.SetActive(false);
        vegParent.SetActive(false);
        sumParent.SetActive(true);

        meatSumTXT.text = "Meat Weight: " + meatWeight + "      Desired Weight: " + 1;
        carbSumTXT.text = "Carb Weight: " + carbWeight + "      Desired Weight: " + 2;
        vegSumTXT.text = "Vegetable Weight: " + vegWeight + "      Desired Weight: " + 3;
        calSumTXT.text = "Total Calories: " + meatCal + carbCal + vegWeight;
    }

    //gets the veg name from currently selected button
    public void setVegName()
    {
        vegName = EventSystem.current.currentSelectedGameObject.name;
    }

    public void openRecipeBook()
    {
        //onclick, activate ui object for recipe book
    }
}
