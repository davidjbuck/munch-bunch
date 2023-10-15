using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sliceObject : MonoBehaviour
{
    Transform planeObject;
    public Transform planeObjectSmall;
    public Transform planeObjectMed;
    public Transform planeObjectLarge;
    public GameObject sliceO;
    public Material middleGuts;
    string sliceOName;
    int meatAmount = 0;

    private void Update()
    {
         //if (Input.GetKeyDown(KeyCode.Space))
         //{
         //    Slice(sliceO);
         //}
    }

    public void getSliceObject(string foodName)
    {
        sliceOName = foodName + "(Clone)";
        sliceO = GameObject.Find(sliceOName);
    }

    public void slicePortionSmall()
    {
        Debug.Log("clicked");
        planeObject = planeObjectSmall;
        meatAmount = 1;
        Slice(sliceO);
    }
    public void slicePortionMed()
    {
        planeObject = planeObjectMed;
        meatAmount = 2;
        Slice(sliceO);
    }
    public void slicePortionLarge()
    {
        planeObject = planeObjectLarge;
        meatAmount = 3;
        Slice(sliceO);
    }

    public void Slice(GameObject o)
    {
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
