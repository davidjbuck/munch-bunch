using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMoveUp : MonoBehaviour
{
    // Speed at which the object will move up
    public float moveSpeed = 0.1f;

    // The starting position of the object
    private Vector3 startPosition;

    // The target position of the object
    private Vector3 targetPosition;

    // Reference to the parent object
    public GameObject parentObject;

    // Reference to the camera to activate
    //CAMERA FOR PLAYERS FACE HERE
    //public Camera newCamera;
    private bool moving;
    // Start is called before the first frame update
    void Start()
    {
        moving = true;
        // Store the starting position of the object
        startPosition = transform.position;

        // Calculate the target position of the object (upwards)
        targetPosition = startPosition + Vector3.up * 2f; // Adjust 2f to the desired height

        // Start the process after 3 seconds
        Invoke("SwitchCameraAndDeactivate", 4f);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the parent object is active
        if (parentObject != null && parentObject.activeInHierarchy && moving)
        {
            // Move the object upwards gradually
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    // Method to switch camera and deactivate parent object
    void SwitchCameraAndDeactivate()
    {
        // Deactivate the parent object
        if (parentObject != null)
        {
            moving = false;
            SceneManager.LoadScene(8);
            // parentObject.SetActive(false);
        }


        //SWITCH TO SOMETHING ELSE HERE

        /*
        // Activate the new camera FOR PLAYER FACE HERE
        if (newCamera != null)
        {
            newCamera.gameObject.SetActive(true);
        }
        */
    }
}