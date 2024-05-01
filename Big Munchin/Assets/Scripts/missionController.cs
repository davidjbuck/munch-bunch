using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class missionController : MonoBehaviour
{
    public TextMeshProUGUI missionTXT;
    public TextMeshProUGUI sideMissionTXT;
    public int currentMission = 0;
    public int currentSideMission;

    public GameObject detective;


    //mission variables
    private int mTwoCounter = 0;
    private int mFourCounter = 0;
    
    
    //side mission variables
    private int smZeroCounter = 0;


    //sets current mission to saved prefab
    public void Start()
    {
        setCurrentMission(1);
        currentMission = PlayerPrefs.GetInt("mission", currentMission);
        Debug.Log("current mission: " + currentMission);
        missionControl(currentMission);

    }

    //~MISSIONS~

    //toggles visibility of mission text
    public void toggleVisibility(bool b)
    {
        if (b == false)
        {
            missionTXT.text = "";
        }
        else if (b == true)
        {
            missionControl(currentMission);
        }
    }

    //getter for current mission
    public int getCurrentMission()
    {
        return currentMission;
    }

    //setter for current mission (also saves in prefab)
    public void setCurrentMission(int i)
    {
        currentMission = i;
        PlayerPrefs.SetInt("mission", currentMission);
        missionControl(currentMission);
    }

    //controller for mission two (increases when player interacts with market vendors)
    public void missionTwoFunction()
    {
        mTwoCounter++;
        Debug.Log("counter for 2: " + mTwoCounter);
        if (mTwoCounter >= 4)
        {
            setCurrentMission(3);
        }
    }
    
    //controller for mission four (collect fried food animals)
    public void missionFourFunction()
    {
        mFourCounter++;
        Debug.Log("counter for 4: " + mFourCounter);
        if (mFourCounter >= 2)
        {
            setCurrentMission(5);
        }
    }

    //controls which mission text is currently displayed
    public void missionControl(int m)
    {
        switch (m)
        {
            case 0:
                missionTXT.text = "Current Mission: Cook A Meal";
                break;
            case 1:
                missionTXT.text = "Current Mission: Take Out The Goons";
                break;
            case 2:
                missionTXT.text = "Current Mission: Check Out The Market";
                
                //sets the first few lines of dialogue
                Dialogue detectiveControl = detective.GetComponent<Dialogue>();
                detectiveControl.currentIndex = 0;
                detectiveControl.endIndex = 4;
                break;
            case 3:
                missionTXT.text = "Current Mission: Return To The Detective";
                
                //sets the next few lines of dialogue
                detectiveControl = detective.GetComponent<Dialogue>();
                detectiveControl.currentIndex = 5;
                detectiveControl.endIndex = 9;
                break;
            case 4:
                missionTXT.text = "Current Mission: Collect Fried Food Animals";
                break;
            case 5:
                missionTXT.text = "Current Mission: Return To The Detective";

                //sets the next few lines of dialogue
                detectiveControl = detective.GetComponent<Dialogue>();
                detectiveControl.currentIndex = 10;
                detectiveControl.endIndex = 27;
                break;
            case 6:
                missionTXT.text = "Current Mission: Investigate the Chicken Farm";
                break;
            case 7:
                missionTXT.text = "Current Mission: Clear the Chicken Farm";
                break;
            case 8:
                missionTXT.text = "Current Mission: Return To The Detective";

                //sets the next few lines of dialogue
                detectiveControl = detective.GetComponent<Dialogue>();
                detectiveControl.currentIndex = 28;
                detectiveControl.endIndex = 35;
                break;
            case 9:
                missionTXT.text = "Investigate Tony's Lair";
                break;
            case 10:
                missionTXT.text = "Fight Big Tony";
                break;
            case 11:
                missionTXT.text = "Handle Tony's Reinforcements";
                break;
            case 12:
                missionTXT.text = "Find a Way to Tony";
                break;
            case 13:
                missionTXT.text = "Take Out the Trash";
                break;
        }
    }




    //~SIDE MISSIONS~

    public int getCurrentSideMission()
    {
        return currentSideMission;
    }
    public void setCurrentSideMission(int i)
    {
        currentSideMission = i;
        sideMissionControl(currentSideMission);
    }

    public void toggleVisibilitySM(bool b)
    {
        if (b == false)
        {
            sideMissionTXT.text = "";
        }
        else if (b == true)
        {
            sideMissionControl(currentSideMission);
        }
    }

    //gather broccoli
    public void sMissionZeroFunction()
    {
        smZeroCounter++;
        Debug.Log("counter for broccoli: " + smZeroCounter);
        if (smZeroCounter >= 4)
        {
            sideMissionControl(1);
        }
    }

    //public void sMissionOneFunction()
    //{
    //    smOneCounter++;
    //    Debug.Log("counter: " + smOneCounter);
    //    if (smOneCounter >= 4)
    //    {
    //        setCurrentMission(3);
    //    }
    //}

    public void sideMissionControl(int m)
    {
        switch (m)
        {
            case 0:
                sideMissionTXT.text = "Side Mission: Gather Broccoli For Vendor";
                break;
            case 1:
                sideMissionTXT.text = "Side Mission: Bring Broccoli To Vendor";
                break;
        }
    }
}

