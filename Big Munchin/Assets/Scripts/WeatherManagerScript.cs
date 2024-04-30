using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManagerScript : MonoBehaviour
{
    public Material skybox1;
    public Material skybox2;
    public Material skybox3;
    // Start is called before the first frame update
    void Start()
    {
        int boxNumber = Random.Range(1, 4);
        Debug.Log("Skybox Number " + boxNumber);
        switch(boxNumber)
        {
            case 1:
                RenderSettings.skybox = skybox1;
                break;
            case 2:
                RenderSettings.skybox = skybox2;
                break;
            case 3:
                RenderSettings.skybox = skybox3;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
