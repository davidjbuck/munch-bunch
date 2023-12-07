using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliceForTony : MonoBehaviour
{
    public Transform planeObject;
    public GameObject planeAsWell;
    public GameObject tonySlice;
    public Material middleGuts;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Slice(tonySlice);
        }
    }

    public void Slice(GameObject o)
    {
        planeAsWell.SetActive(true);
        o = tonySlice;
        SlicedHull hull = o.Slice(planeObject.position, planeObject.up);
        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(o, middleGuts);
            hullSetups(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(o, middleGuts);
            hullSetups(lowerHull);

            Destroy(o);
        }
        Debug.Log("slice called");
        planeAsWell.SetActive(false);
    }

    public void hullSetups(GameObject o)
    {
        o.AddComponent<Rigidbody>();
        MeshCollider collider = o.AddComponent<MeshCollider>();
        collider.convex = true;
    }
}
