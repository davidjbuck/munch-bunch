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

    float meatWeight;
    float carbWeight;
    float vegWeight;
    float totalWeight;
    float meatCal;
    float carbCal;
    float vegCal;
    float totalCal;

    public GameObject sliceButtonHolder;
    public GameObject slicePlaneHolder;

    public GameObject meatParent;
    public GameObject carbParent;
    public GameObject vegParent;
    public GameObject sumParent;

    public GameObject riceParent;
    public ParticleSystem ricePour;
    float fakeWeightCount = 0;
    public TextMeshProUGUI weightTxt;

    string vegName;
    public GameObject plate1;
    public GameObject plate2;
    public TextMeshProUGUI mostPlateWeightTxt;
    public TextMeshProUGUI vegWeightTxt;

    public TextMeshProUGUI meatSumTXT;
    public TextMeshProUGUI carbSumTXT;
    public TextMeshProUGUI vegSumTXT;
    public TextMeshProUGUI calSumTXT;

    private void Update()
    {
        if(currentScreen == 1)
        {
            //particle system timing
            if (Input.GetMouseButton(0))
            {
                if (riceParent.activeInHierarchy == true)
                {
                    fakeWeightCount += Time.deltaTime * 2;
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

        if (currentScreen == 3)
        {
            Destroy(GameObject.Find("brocoli(Clone)"));
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

    public void setMeatCounts(float w, float c)
    {
        meatWeight = w;
        meatCal = c;
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
        carbWeight = fakeWeightCount;
        carbCal = fakeWeightCount * 2;
        currentScreen = 2;
        plate1.SetActive(true);
        plate2.SetActive(true);
        mostPlateWeightTxt.text = "Current Weight: " + (meatWeight + carbWeight);
        riceParent.SetActive(false);
        carbParent.SetActive(false);
        vegParent.SetActive(true);
    }

    public void switchToSummary()
    {
        currentScreen = 3;
        plate1.SetActive(false);
        plate2.SetActive(false);
        vegParent.SetActive(false);
        sumParent.SetActive(true);

        meatSumTXT.text = "Meat Weight: " + meatWeight + "      Desired Weight: " + 1;
        carbSumTXT.text = "Carb Weight: " + carbWeight + "      Desired Weight: " + 2;
        vegSumTXT.text = "Vegetable Weight: " + vegWeight + "      Desired Weight: " + 3;
        calSumTXT.text = "Total Calories: " + meatCal + carbCal + vegWeight;
    }

    public void setVegName()
    {
        vegName = EventSystem.current.currentSelectedGameObject.name;
    }

    public void openRecipeBook()
    {
        //onclick, activate ui object for recipe book
    }
}
