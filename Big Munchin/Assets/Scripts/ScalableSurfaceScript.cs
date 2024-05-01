using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableSurfaceScript : MonoBehaviour
{
    //point of entry or exit for the player, depending on which way they are coming from
    public Transform accessOne;
    public Transform accessTwo;
    public Transform[] steps;
    public bool positiveTraversalDirection;
    //whether pressing W/D moves the player through the steps array in the positive direction.
    //Should always be true for ladders. If Ledges are behaving opposite of the way expected, then flip this bool
    public SurfaceType typeOfSurface;//which type of surface this object is
    public int currentStep;
    public float stepLength;
    private float lastStepTime;
    private Vector3 prevTarget;
    private Transform lerpTarget;
    private Vector3 currentTarget;
    public float exitTimeLength;
    private bool goingPositiveDirection;
    private bool doneAttaching = false;

    public void Start()
    {
        lastStepTime = Time.time;
    }

    //an enum that fefines whether the player moves in vertical or horizontal steps across scalable surfaces
    public enum SurfaceType
    {
        Ladder = 0,
        Ledge =1
    }

    public void AttachToSurface(Vector3 playerLocation)
    {
        currentTarget = playerLocation;
        if(typeOfSurface == SurfaceType.Ladder)
        {
            //Debug.Log("AccessOne: " + Mathf.Abs(Vector3.Distance(accessOne.transform.position, playerLocation)));
            //Debug.Log("AccessTwo: " + Mathf.Abs(Vector3.Distance(accessTwo.transform.position, playerLocation)));
            if(Mathf.Abs(Vector3.Distance(accessOne.transform.position, playerLocation)) < Mathf.Abs(Vector3.Distance(accessTwo.transform.position, playerLocation)))
            {
                currentStep = 0;
                prevTarget = playerLocation;
                lastStepTime = Time.time;
            }
            else
            {
                currentStep = steps.Length-1;
                prevTarget = playerLocation;
                lastStepTime = Time.time;
            }
            lerpTarget = steps[currentStep].transform;
        }
        else//is Ledge
        {
            float min = Mathf.Abs(Vector3.Distance(steps[0].transform.position, playerLocation));
            int minIndex = 0;
            for(int i = 1; i < steps.Length; i++)
            {
                if(Mathf.Abs(Vector3.Distance(steps[i].transform.position, playerLocation)) < min)
                {
                    minIndex = i;
                    min = Mathf.Abs(Vector3.Distance(steps[i].transform.position, playerLocation));
                }
            }
            currentStep = minIndex;
            lastStepTime = Time.time;
            prevTarget = playerLocation;
            lerpTarget = steps[currentStep].transform;            
            //Debug.Log("Attaching to surface on step " + currentStep);
        }
        doneAttaching = false;
    }

    public void Update()
    {
        if(Time.time - lastStepTime > stepLength)
        {
            doneAttaching = true;
        }
    }

    public void AdvanceStep(int amt)
    {
        if (Time.time - lastStepTime > stepLength && !(currentStep<0||currentStep>steps.Length-1))
        {
            if(amt>0)
            {
                goingPositiveDirection = true;
            }
            else
            {
                goingPositiveDirection = false;
            }
            //increment step
            //Debug.Log("Advancing Step to "+currentStep);
            if (positiveTraversalDirection)
            {
                currentStep+=amt;
                lastStepTime = Time.time;
            }
            else
            {
                currentStep-=amt;
                lastStepTime= Time.time;
            }

            //assign new lerpTargets
            if (currentStep < 0 ||currentStep > steps.Length-1) { 
                if(currentStep < 0)
                {
                    prevTarget = lerpTarget.position;
                    lerpTarget = accessOne.transform;
                }
                else if(currentStep > steps.Length-1)
                {
                    prevTarget = lerpTarget.position;
                    lerpTarget = accessTwo.transform;
                }
            }
            else
            {
                prevTarget = lerpTarget.position;
                lerpTarget = steps[currentStep].transform;
            }
        }

        //put code here to trigger next animation/SFX once animations are done

    }

    public Vector3 GetLerpTarget() 
    {
        Vector3 pos = LerpVector3(prevTarget, lerpTarget.position, lastStepTime, stepLength);
        currentTarget = pos;
        return currentTarget;
    
    }

    public bool EndReached()
    {
        if(Time.time - lastStepTime > exitTimeLength && (currentStep<0||currentStep>steps.Length-1))
        {
            //Debug.Log("End reached");
            return true;
        }
        else
        {
            return false;
        }
    }

    float Lerp(float startValue, float endValue, float startTime, float lerpTime)
    {

        float timeElapsed = Time.time - startTime;
        //Debug.Log(timeElapsed);
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

    public Vector3 LerpVector3(Vector3 startVector, Vector3 endVector, float startTime, float endTime)
    {
        float x = Lerp(startVector.x, endVector.x, startTime, endTime);
        float y = Lerp(startVector.y, endVector.y, startTime, endTime);
        float z = Lerp(startVector.z, endVector.z, startTime, endTime);
        return new Vector3(x, y, z);
    }

    public bool CanAdvance()
    {
        if(Time.time - lastStepTime > stepLength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetGoingPositiveDirection()
    {
        return goingPositiveDirection;
    }

    public bool GetDoneAttaching()
    {
        return doneAttaching;
    }
}
