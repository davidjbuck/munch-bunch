using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class missionController : MonoBehaviour
{
    public TextMeshProUGUI missionTXT;
    public int currentMission = 0;

    private int mZeroCounter = 0;
    private int mOneCounter = 0;


    public void Start()
    {
        setCurrentMission(2);
        currentMission = PlayerPrefs.GetInt("mission", currentMission);
        Debug.Log("current mission: " + currentMission);
        missionControl(currentMission);

    }
    public int getCurrentMission()
    {
        return currentMission;
    }
    public void setCurrentMission(int i)
    {
        currentMission = i;
        PlayerPrefs.SetInt("mission", currentMission);
        missionControl(currentMission);
    }
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

    public void mTwoFunction()
    {
        mZeroCounter++;
        Debug.Log("counter: " +  mZeroCounter);
        if (mZeroCounter >= 4)
        {
            setCurrentMission(3);
        }
    }


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
}

