using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveBack : MonoBehaviour
{
    // Speed at which the object will move back
    public float moveSpeed = 0.1f;

    // The starting position of the object
    private Vector3 startPosition;

    // The target position of the object
    private Vector3 targetPosition;

    // Reference to the parent object
    public GameObject parentObject;

    // Timer to track elapsed time
    private float timer = 0f;

    // Total duration for moving back
    private float moveBackDuration = 3f;

    // Flag to indicate if the timer has started
    private bool timerStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        // Store the starting position of the object
        startPosition = transform.position;

        // Calculate the target position of the object (original position)
        targetPosition = startPosition + Vector3.left * 2f; // Adjust 2f to the desired height
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the parent object is active
        if (parentObject != null && parentObject.activeInHierarchy)
        {
            // If timer has not started, start the timer
            if (!timerStarted)
            {
                timerStarted = true;
                timer = 0f;
            }

            // Increment the timer
            timer += Time.deltaTime;

            // Check if the move back duration is reached
            if (timer >= moveBackDuration)
            {
                // Stop moving the object
                enabled = false;
            }
            else
            {
                // Move the object back to its starting position gradually
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            // Reset timer when parent object is not active
            timerStarted = false;
        }
    }
}