using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectMouseDrag : MonoBehaviour
{
    public Camera cam;

    private void OnMouseDrag()
    {
        transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 6));
    }
}
