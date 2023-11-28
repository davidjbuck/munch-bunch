using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCameraDirector : MonoBehaviour
{
    Camera cam;
    Camera mc;
    Cutscene cs;
    public bool cutsceneOngoing = false;
    public bool testing = false;
    public bool preview = false;
    public int numOperations;//how many operations the cutscene has
    public int[] operationsPerformed;//how many operations to perform at a time for each operation, there should be an int corresponding to each operation, with parallel operations always in descending order of operationsPerformed as you progress through the queue
    public Cutscene.OperationType[] opTypes;//operation type, should run parallel to the other single capacity arrays, with as amny as there are operations
    public Cutscene.TerminationType[] termTypes;//termination type of the operation, should run parallel to the other single capacity arrays, with as amny as there are operations
    public float[] operationDurations;//how long each operation should take. Ignored if dialog triggers progression. Corresponds to the speed field of the Cutscene class
    public Vector3[] targetVectors;//does not run parallel to the others. Each operation gets 4 target Vector3 allocations, even though most only use 2, so it should be exactly 4 times the length of numOperations

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        mc = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (testing)
        {
            if (preview)
            {
                Setup("not test");
            }
            else
            {
                Setup("TEST");
            }
            Setup("TEST");
            testing = false;
        }
        if (cutsceneOngoing && cs.currentOperation < cs.csOps.Length)
        {
            if (cs.csOps[cs.currentOperation].numOpsPerformed == 1)
            {
                PerformOperation(cs.currentOperation);
                if (cs.currentOperation >= cs.csOps.Length)
                {
                    cutsceneOngoing = false;
                    Cleanup();
                    return;
                }
            }
            else
            {
                for (int i = cs.currentOperation; i < cs.currentOperation + cs.csOps[cs.currentOperation].numOpsPerformed; i++)
                {
                    if (!cs.operationsFinished[i])
                    {
                        PerformOperation(i);
                        if (cs.currentOperation >= cs.csOps.Length)
                        {
                            cutsceneOngoing = false;
                            Cleanup();
                            return;
                        }
                    }
                }
            }
        }
    }

    public void Setup(String cutsceneName)
    {
        cam.enabled = true;
        mc.enabled = false;
        cutsceneOngoing = true;
        if(cutsceneName == "TEST") 
        {
            //LoadTestCutscene();
        }
        else
        {
            LoadCutscene("TEST");
        }
        
    }

    public void Cleanup()
    {
        cam.enabled = false;
        mc.enabled = true;
    }



    public void LoadCutscene(String cutsceneName)
    {
        Cutscene.Operation[] opsQueue = new Cutscene.Operation[numOperations];
        for (int i = 0; i < numOperations; i++)
        {
            opsQueue[i] = new Cutscene.Operation();
            opsQueue[i].numOpsPerformed = operationsPerformed[i];
            opsQueue[i].opType = opTypes[i];
            opsQueue[i].termType = termTypes[i];
            opsQueue[i].speed = operationDurations[i];
            opsQueue[i].targets = new Vector3[4];
            for(int j = 0; j < 4; j++)
            {
                opsQueue[i].targets[j] = targetVectors[i*4+j];
            }
        }
        cs = new Cutscene(opsQueue);
    }

    //put logic to handle each operation here, check if termination condition has been reached,
    //and call Cutscene's TerminateOperation method once each has concluded
    public void PerformOperation(int index)
    {
        switch ((int)cs.csOps[index].opType)
        {
            case 0://CutTo
                gameObject.transform.position = cs.csOps[cs.currentOperation].targets[0];//set position
                gameObject.transform.rotation = Quaternion.Euler(cs.csOps[cs.currentOperation].targets[1]);//set rotation
                //put logic to check termination condition here
                if(Time.time> cs.csOps[index].opStartTime + cs.csOps[index].speed && ((int)cs.csOps[index].termType!=0))
                {
                    cs.TerminateOperation(index);//mark operation done
                }
                break;
            case 1://LerpTo
                float x = Lerp(cs.csOps[index].targets[0].x, cs.csOps[index].targets[1].x, cs.csOps[index].opStartTime, cs.csOps[index].speed);
                float y = Lerp(cs.csOps[index].targets[0].y, cs.csOps[index].targets[1].y, cs.csOps[index].opStartTime, cs.csOps[index].speed);
                float z = Lerp(cs.csOps[index].targets[0].z, cs.csOps[index].targets[1].z, cs.csOps[index].opStartTime, cs.csOps[index].speed);
                gameObject.transform.position = new Vector3(x, y, z);
                if(Time.time> cs.csOps[index].opStartTime+ cs.csOps[index].speed && ((int)cs.csOps[index].termType != 0))
                {
                    gameObject.transform.position = cs.csOps[index].targets[1];//set to the end position
                    cs.TerminateOperation(index);//mark operation done
                }
                break;
            case 2://
                float startTime = cs.csOps[index].opStartTime;
                float curTime = Time.time;
                float time = (curTime- startTime)/ cs.csOps[index].speed;
                float endTime = startTime + cs.csOps[index].speed;
                if (curTime > endTime && ((int)cs.csOps[index].termType != 0))
                {
                    gameObject.transform.position = cs.csOps[index].targets[3];//set to the end position
                    cs.TerminateOperation(index);//mark operation done
                }
                else
                {
                    gameObject.transform.position = BezierCurve(cs.csOps[index].targets[0], cs.csOps[index].targets[1], cs.csOps[index].targets[2], cs.csOps[index].targets[3], time);
                }
                break;
            case 3://PanTo
                if (Time.time > cs.csOps[index].opStartTime + cs.csOps[index].speed && ((int)cs.csOps[index].termType != 0))
                {
                    gameObject.transform.rotation = Quaternion.Euler(cs.csOps[index].targets[1]);
                    cs.TerminateOperation(index);
                }
                else
                {
                    float xRot = Lerp(cs.csOps[index].targets[0].x, cs.csOps[index].targets[1].x, cs.csOps[index].opStartTime, cs.csOps[index].speed);
                    float yRot = Lerp(cs.csOps[index].targets[0].y, cs.csOps[index].targets[1].y, cs.csOps[index].opStartTime, cs.csOps[index].speed);
                    float zRot = Lerp(cs.csOps[index].targets[0].z, cs.csOps[index].targets[1].z, cs.csOps[index].opStartTime, cs.csOps[index].speed);
                    gameObject.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
                }                    
                break;
        }
    }

    float Lerp(float startValue, float endValue, float startTime, float lerpTime)
    {
        float timeElapsed = Time.time - startTime;
        float val;
        if (timeElapsed < lerpTime)
        {
            val = Mathf.Lerp(startValue, endValue, timeElapsed / lerpTime);
        }
        else
        {
            val = endValue;
        }
        return val;
    }

    Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector3 result =
            (u3) * p0 +
            (3f * u2 * t) * p1 +
            (3f * u * t2) * p2 +
            (t3) * p3;

        return result;
    }

    public void EndOperation(int index)
    {
        switch ((int)cs.csOps[cs.currentOperation].opType)
        {
            case 0://CutTo
                cs.TerminateOperation(index);//mark operation done  
                break;
            case 1://LerpTo                
                gameObject.transform.position = cs.csOps[index].targets[1];//set to the end position
                cs.TerminateOperation(index);//mark operation done                
                break;
            case 2://
                gameObject.transform.position = cs.csOps[index].targets[3];//set to the end position
                cs.TerminateOperation(index);//mark operation done
                break;
            case 3://PanTo
                gameObject.transform.rotation = Quaternion.Euler(cs.csOps[index].targets[1]);
                cs.TerminateOperation(index);
                break;
        }
    }
}
