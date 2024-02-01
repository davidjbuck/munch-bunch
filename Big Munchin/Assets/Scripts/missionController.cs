using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class missionController : MonoBehaviour
{
    public int currentMission = 0;
    public TextMeshProUGUI missionTXT;

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
        Debug.Log("current mission: " + currentMission);
        missionControl(currentMission);
    }

    public void missionControl(int m)
    {
        switch (m)
        {
            case 0:
                missionTXT.text = "Current Mission: Interact with Bush";
                break;
            case 1:
                missionTXT.text = "Current Mission: Harvest 3 broccoli";
                break;
            case 2:
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
