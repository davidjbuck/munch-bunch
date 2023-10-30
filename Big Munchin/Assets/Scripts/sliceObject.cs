using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sliceObject : MonoBehaviour
{
    float tempWeight;
    float tempCal;
    public GameObject ngcObject;

    Transform planeObject;
    public GameObject planeObjectSmall;
    public GameObject planeObjectMed;
    public GameObject planeObjectLarge;
    public GameObject sliceO;
    public GameObject sliceButtonHolder;

    public GameObject knife;

    public Material middleGuts;
    string sliceOName;

    public void getSliceObject(string foodName)
    {
        sliceOName = foodName + "(Clone)";
        sliceO = GameObject.Find(sliceOName);
    }

    public void slicePortionSmall()
    {
        planeObject = planeObjectSmall.transform;
        tempWeight = sliceO.GetComponent<IngredientNMG>().weight / 3;
        tempCal = sliceO.GetComponent<IngredientNMG>().calories / 3;
        ngcObject.GetComponent<NGcontroller>().setMeatCounts(tempWeight, tempCal);
        Slice(sliceO);
    }
    public void slicePortionMed()
    {
        planeObject = planeObjectMed.transform;
        tempWeight = sliceO.GetComponent<IngredientNMG>().weight / 2;
        tempCal = sliceO.GetComponent<IngredientNMG>().calories / 2;
        ngcObject.GetComponent<NGcontroller>().setMeatCounts(tempWeight, tempCal);
        Slice(sliceO);
    }
    public void slicePortionLarge()
    {
        planeObject = planeObjectLarge.transform;
        tempWeight = sliceO.GetComponent<IngredientNMG>().weight / 1.5f;
        tempCal = sliceO.GetComponent<IngredientNMG>().calories / 1.5f;
        ngcObject.GetComponent<NGcontroller>().setMeatCounts(tempWeight, tempCal);
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
        knife.SetActive(false);
    }

    public void hullSetups(GameObject o)
    {
        o.AddComponent<Rigidbody>();
        MeshCollider collider = o.AddComponent<MeshCollider>();
        collider.convex = true;
    }
}
