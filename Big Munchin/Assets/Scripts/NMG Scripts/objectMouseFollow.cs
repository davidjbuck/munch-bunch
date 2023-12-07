using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectMouseFollow : MonoBehaviour
{
    public Camera cam;

    private void Update()
    {
        transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
    }
}
