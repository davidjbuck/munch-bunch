using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class missionController : MonoBehaviour
{
    public int currentMission = 0;
    public TextMeshProUGUI missionTXT;

    private int mZeroCounter = 0;
    private int mOneCounter = 0;

    public void Start()
    {
        missionControl(currentMission);
    }
    public int getCurrentMission()
    {
        return currentMission;
    }
    public void setCurrentMission(int i)
    {
        currentMission = i;
        missionControl(currentMission);
    }
    public void mZeroFunction()
    {
        mZeroCounter++;
        if (mZeroCounter == 2)
        {
            setCurrentMission(1);
        }
    }
    public void mOneFunction()
    {
        mOneCounter++;
        if (mOneCounter == 3)
        {
            setCurrentMission(2);
        }
    }


    public void missionControl(int m)
    {
        switch (m)
        {
            case 0:
                missionTXT.text = "Current Mission: Interact with 2 Bushes";
                break;
            case 1:
                missionTXT.text = "Current Mission: Harvest 3 broccoli";
                break;
            case 2:
                missionTXT.text = "Current Mission: Celebrate, You Did Things";
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
        }
    }
}
