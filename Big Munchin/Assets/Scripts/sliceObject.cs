using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliceObject : MonoBehaviour
{
    public Transform planeObject;
    public GameObject sliceO;
    public Material middleGuts;
    string sliceOName;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Slice(sliceO);
        }
    }
    public void getSliceObject(string foodName)
    {
        sliceOName = foodName + "(Clone)";
        sliceO = GameObject.Find(sliceOName);
    }

    public void Slice(GameObject o)
    {
        SlicedHull hull = o.Slice(planeObject.position, planeObject.up);
        Debug.Log("name: " + sliceOName);
        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(o, middleGuts);
            GameObject lowerHull = hull.CreateLowerHull(o, middleGuts);

            Destroy(o);
        }
    }
}
