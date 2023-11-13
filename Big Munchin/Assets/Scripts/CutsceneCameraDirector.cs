using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCameraDirector : MonoBehaviour
{
    Camera cam;
    Camera mc;
    Cutscene cs;
    bool cutsceneOngoing = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        mc = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cutsceneOngoing)
        {
            if (cs.csOps[cs.currentOperation].numOpsPerformed == 1)
            {
                PerformOperation(cs.currentOperation);
            }
            else
            {
                for (int i = cs.currentOperation; i < cs.currentOperation + cs.csOps[cs.currentOperation].numOpsPerformed; i++)
                {
                    if (!cs.operationsFinished[i])
                    {
                        PerformOperation(i);
                    }
                }
            }
        }
    }

    public void Setup(int cutsceneNum)
    {
        cam.enabled = true;
        mc.enabled = false;
        cutsceneOngoing = true;
        LoadCutscene(cutsceneNum);
    }

    public void Cleanup()
    {
        cam.enabled = false;
        mc.enabled = true;
    }

    //update this method later to actually add fetching from the flat file once serializing is added, for now just use hard-coded test Cutscenes
    public void LoadCutscene(int cutsceneID)
    {
        //cs = new Cutscene();
    }

    //put logic to handle each operation here and call Cutscene's TerminateOperation method once each has concluded
    public void PerformOperation(int index)
    {

    }
}
