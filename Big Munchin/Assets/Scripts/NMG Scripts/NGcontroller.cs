using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NGcontroller : MonoBehaviour
{
    //stores the int for the current screen, the object holding this script, and the current object
    int currentScreen = 0;
    public GameObject counter;
    string currentMade;

    //holds the variables for the calories and weights of each object (and total)
    float meatWeight;
    float carbWeight;
    float vegWeight = 0;
    float totalWeight;
    float meatCal;
    float carbCal;
    float vegCal;
    float totalCal;

    //objects for each screen parent
    public GameObject tutorialParent;
    public GameObject meatParent;
    public GameObject carbParent;
    public GameObject vegParent;
    public GameObject sumParent;

    //texts for tutorial tips
    public TextMeshProUGUI meatTutorialTXT;
    public TextMeshProUGUI carbTutorialTXT;
    public TextMeshProUGUI vegTutorialTXT;
    

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
    public GameObject bowlForRice;
    public GameObject scoopForRice;

    //objects for the veg screen
    string vegName;
    public GameObject bowl;
    public GameObject bowlOnScale;
    public TextMeshProUGUI mostPlateWeightTxt;
    public TextMeshProUGUI vegWeightTxt;

    //objects for summary screen
    public TextMeshProUGUI meatSumTXT;
    public TextMeshProUGUI carbSumTXT;
    public TextMeshProUGUI vegSumTXT;
    public TextMeshProUGUI calSumTXT;
    public GameObject star;
    public GameObject star1;
    public GameObject star2;
    public TextMeshProUGUI cityHealthTXT;
    int cityHealth = 0;

    public GameObject startCanvas;
    public GameObject NMGCanvas;

    public GameObject player;
    public GameObject playerForInventory;
    public GameObject kitchenCam;

    public GameObject missionCont;

    public GameObject saveLoad;


    private void Start()
    {
        missionCont.GetComponent<missionController>().setCurrentMission(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            startCanvas.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            startCanvas.SetActive(false);
        }
    }

    public void startCooking()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        startCanvas.SetActive(false);
        NMGCanvas.SetActive(true);
        AudioListener[] al = player.GetComponentsInChildren<AudioListener>();
        foreach(AudioListener listener in al)
        {
            listener.enabled = false;
        }
        player.SetActive(false);
        kitchenCam.SetActive(true);

        Physics.gravity /= 7.5f;
    }

    public void stopCooking()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        NMGCanvas.SetActive(false);
        player.SetActive(true);
        kitchenCam.SetActive(false);
        startCanvas.SetActive(false);

        saveLoad.GetComponent<SaveLoad>().SaveInventory();

        Physics.gravity *= 7.5f;
        GameObject cutsceneCamera = GameObject.Find("Cutscene Camera");
        AudioListener[] al = player.GetComponentsInChildren<AudioListener>();
        foreach (AudioListener listener in al)
        {
            listener.enabled = true;
        }
        cutsceneCamera.GetComponent<CutsceneCameraDirector>().Setup("not test");
    }
    

    private void Update()
    {
        //if we're on the carb screen
        if (currentScreen == 1)
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

        //on summary screen, destroys all created objects
        if (currentScreen == 3)
        {
            Destroy(GameObject.Find("brocoli(Clone)"));
        }
    }

    //gets the name of the selected button and instantiates that object
    public void createObjectMeat()
    {
        meatTutorialTXT.text = "Next, choose your portion by selecting the buttons under the food (in general, half of the meat is a good portion). Once satisfied, hit next";
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
        carbTutorialTXT.text = "Now hold down to pour the rice. Continue pouring to desired amount (around 100g of carbs are recommended). Once satisfied, hit next";
        riceParent.SetActive(true);
        scoopForRice.SetActive(true);
    }

    public void switchToMeat()
    {
        currentScreen = 0;
        tutorialParent.SetActive(false);
        meatParent.SetActive(true);
    }

    //switches screens from meat to carb (and activates/deactivates respective objects)
    public void switchToCarb()
    {
        carbTutorialTXT.text = "Same as last, select your carb from the buttons on the left";
        currentScreen = 1;
        Destroy(GameObject.Find("Upper_Hull"));
        Destroy(GameObject.Find("Lower_Hull"));
        scaleObject.SetActive(true);
        bowlForRice.SetActive(true);
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
        bowl.SetActive(true);
        bowlOnScale.SetActive(true);
        bowlForRice.SetActive(false);
        scoopForRice.SetActive(false);
        riceParent.SetActive(false);
        carbParent.SetActive(false);
        vegParent.SetActive(true);
    }

    //switches screens from veg to sum (and activates/deactivates respective objects)
    public void switchToSummary()
    {
        currentScreen = 3;
        scaleObject.SetActive(false);
        bowl.SetActive(false);
        bowlOnScale.SetActive(false);
        vegParent.SetActive(false);
        sumParent.SetActive(true);
        

        //displays all the values for summary
        meatSumTXT.text = "Meat Calories: " + meatCal + "      Desired Calories: " + 230;
        carbSumTXT.text = "Carb Calories: " + carbCal.ToString("F2") + "      Desired Calories: " + 4;
        vegSumTXT.text = "Vegetable Calories: " + vegWeight + "      Desired Calories: " + (meatWeight + carbWeight).ToString("F2");
        calSumTXT.text = "Total Calories: " + (meatCal + carbCal + vegWeight).ToString("F2");
         
        //activates stars depending how well the numbers are balanced
        if (Math.Abs((230 - meatCal)) <= 10)
        {
            star.SetActive(true);
            cityHealth += 10;
        }
        if (Math.Abs((4 - carbCal)) <= 1)
        {
            star1.SetActive(true);
            cityHealth += 10;
        }
        if (Math.Abs(((meatWeight + carbWeight) - vegWeight)) <= 1)
        {
            star2.SetActive(true);
            cityHealth += 10;
        }

        cityHealthTXT.text = "City Health Increased by " + cityHealth;

        Item completeMeal = new Item();
        completeMeal.itemName = "Chicken, Rice, and Broccoli Meal";
        completeMeal.flavorText = "A completed dish. Provides many advantages over snacking.";
        completeMeal.amount = 1;
        playerForInventory.GetComponent<Inventory>().addItem(completeMeal);
    }


    public void createObjectVeg()
    {
        vegTutorialTXT.text = "Now, drag the desired amount of the vegetable to the scale (the vegetables should match the weight of the rest of the meal). Once satisfied, hit next ";
        
        //gets string of current button and compares it against "currentMade" to make sure its not the same
        string foodItem = EventSystem.current.currentSelectedGameObject.name;
        if (foodItem != currentMade)
        {
            Instantiate(Resources.Load(foodItem), new Vector3(-402.3f, 30f, 64.88f), Quaternion.identity);
            currentMade = foodItem;
        }
    }

    public void getVegWeight(float w)
    {
        vegWeight = w;
        vegWeightTxt.text = vegWeight.ToString("F2");
    }

    public float setVegWeight()
    {
        return vegWeight;
    }

    public void openRecipeBook()
    {
        //onclick, activate ui object for recipe book
    }
}
