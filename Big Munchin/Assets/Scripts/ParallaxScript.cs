using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    Material mat;
    public float rate;
    GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        mat = gameObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = cam.transform.position;
        Vector3 objPos = gameObject.transform.position;
        Vector3 offset = camPos - objPos;
        Vector2 result = Vector2.right * offset.x * rate;
        //Debug.Log(result);
        mat.mainTextureOffset = result;
        
    }
}
