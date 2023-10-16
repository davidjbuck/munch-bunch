using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sliceObject : MonoBehaviour
{
    Transform planeObject;
    public GameObject planeObjectSmall;
    public GameObject planeObjectMed;
    public GameObject planeObjectLarge;
    public GameObject sliceO;
    public GameObject sliceButtonHolder;

    public Material middleGuts;
    string sliceOName;
    int meatAmount = 0;

    public void getSliceObject(string foodName)
    {
        sliceOName = foodName + "(Clone)";
        sliceO = GameObject.Find(sliceOName);
    }

    public void slicePortionSmall()
    {
        planeObject = planeObjectSmall.transform;
        meatAmount = 1;
        Slice(sliceO);
    }
    public void slicePortionMed()
    {
        planeObject = planeObjectMed.transform;
        meatAmount = 2;
        Slice(sliceO);
    }
    public void slicePortionLarge()
    {
        planeObject = planeObjectLarge.transform;
        meatAmount = 3;
        Slice(sliceO);
    }

    public void Slice(GameObject o)
    {
        planeObjectSmall.SetActive(false);
        planeObjectMed.SetActive(false);
        planeObjectLarge.SetActive(false);
        sliceButtonHolder.SetActive(false);

        SlicedHull hull = o.Slice(planeObject.position, planeObject.up);
        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(o, middleGuts);
            hullSetups(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(o, middleGuts);
            hullSetups(lowerHull);

            Destroy(o);
        }
    }

    public void hullSetups(GameObject o)
    {
        //Rigidbody rb = o.AddComponent<Rigidbody>();
        o.AddComponent<Rigidbody>();
        MeshCollider collider = o.AddComponent<MeshCollider>();
        collider.convex = true;
    }
}
