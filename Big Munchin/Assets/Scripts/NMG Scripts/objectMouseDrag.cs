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
        transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zpos));
    }

    private void OnCollisionEnter(Collision collision)
    {
        ngControllerParent = GameObject.Find("NMG Parent");
        vegWeight = ngControllerParent.GetComponent<NGcontroller>().setVegWeight();
        if (collision.gameObject.tag == "scale")
        {
            vegWeight++;
            ngControllerParent.GetComponent<NGcontroller>().getVegWeight(vegWeight);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        ngControllerParent = GameObject.Find("NMG Parent");
        vegWeight = ngControllerParent.GetComponent<NGcontroller>().setVegWeight();
        if (collision.gameObject.tag == "scale")
        {
            vegWeight--;
            ngControllerParent.GetComponent<NGcontroller>().getVegWeight(vegWeight);
        }
    }
}
