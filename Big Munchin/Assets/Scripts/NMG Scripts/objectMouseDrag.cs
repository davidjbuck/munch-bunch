using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class objectMouseDrag : MonoBehaviour
{
    public Camera cam;
    public int zpos = 25;
    public GameObject ngControllerParent;
    float vegWeight;

    private void OnMouseDrag()
    {
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y+10, zpos));
    }

    private void OnTriggerEnter(Collider other)
    {
        ngControllerParent = GameObject.Find("NMG Parent");
        vegWeight = ngControllerParent.GetComponent<NGcontroller>().setVegWeight();
        if (other.gameObject.tag == "bowlOnScale")
        {
            vegWeight += 20;
            ngControllerParent.GetComponent<NGcontroller>().getVegWeight(vegWeight);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ngControllerParent = GameObject.Find("NMG Parent");
        vegWeight = ngControllerParent.GetComponent<NGcontroller>().setVegWeight();
        if (other.gameObject.tag == "bowlOnScale")
        {
            vegWeight -= 20;
            ngControllerParent.GetComponent<NGcontroller>().getVegWeight(vegWeight);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    ngControllerParent = GameObject.Find("NMG Parent");
    //    vegWeight = ngControllerParent.GetComponent<NGcontroller>().setVegWeight();
    //    if (collision.gameObject.tag == "bowlOnScale")
    //    {
    //        vegWeight += 20;
    //        ngControllerParent.GetComponent<NGcontroller>().getVegWeight(vegWeight);
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    ngControllerParent = GameObject.Find("NMG Parent");
    //    vegWeight = ngControllerParent.GetComponent<NGcontroller>().setVegWeight();
    //    if (collision.gameObject.tag == "bowlOnScale")
    //    {
    //        vegWeight-=20;
    //        ngControllerParent.GetComponent<NGcontroller>().getVegWeight(vegWeight);
    //    }
    //}
}
