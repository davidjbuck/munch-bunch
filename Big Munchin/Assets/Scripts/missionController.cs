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
    
    
    //side mission variables
    private int smZeroCounter = 0;
    private int smOneCounter = 0;


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
        Debug.Log("counter: " + mTwoCounter);
        if (mTwoCounter >= 4)
        {
            setCurrentMission(3);
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
                break;
            case 3:
                missionTXT.text = "Current Mission: Return To The Detective";
                detective.GetComponent<Dialogue>().line[0] = "yargh";
                detective.GetComponent<Dialogue>().line[1] = "I need actual dialogue here to see if it works properly";
                detective.GetComponent<Dialogue>().line[2] = "and also if it moves like how the dialogue should scroll";
                detective.GetComponent<Dialogue>().line[3] = "so there be somethin about some birds or whatnot? pretty nutty if you ask me";
                detective.GetComponent<Dialogue>().StartDialogue();
                break;
            case 4:
                missionTXT.text = "Current Mission: Collect Fried Food Animals";
                break;
            case 5:
                missionTXT.text = "Current Mission: Return To The Detective";
                break;
            case 6:
                missionTXT.text = "Current Mission: Investigate the Chicken Farm";
                break;
            case 7:
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
        Debug.Log("counter: " + smZeroCounter);
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

