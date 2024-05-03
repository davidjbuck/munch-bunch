using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonySink : MonoBehaviour
{
    // Total time taken for sinking
    public float sinkDuration = 6f; // Doubling the duration for slower sinking

    // Speed at which the object will sink
    public float sinkSpeed = 1f;

    // The starting position of the object
    private Vector3 startPosition;

    // The target position of the object
    private Vector3 targetPosition;

    // Timer to track sinking duration
    private float timer = 0f;

    // Cameras
    public Camera camera1;
    public Camera camera2;
    public GameObject fallingTony;
    public AudioSource audioSource;
    public AudioSource audioSource2;

    // Start is called before the first frame update
    void Start()
    {
        // Activate camera1
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
        // Store the starting position of the object
        startPosition = transform.position;

        // Calculate the target position of the object (downwards)
        targetPosition = startPosition - Vector3.up * CalculateSinkDistance();

        // Start the sinking process after 2 seconds
        Invoke("StartSink", 2.2f);
    }

    // Update is called once per frame
    void Update()
    {
        // Increment the timer if the sinking process has started
        if (timer > 0f)
        {
            timer += Time.deltaTime;

            // Calculate the step size based on sink speed and frame rate
            float step = sinkSpeed * Time.deltaTime;

            // Move the object towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            // Check if the object has reached the target position
            if (transform.position == targetPosition)
            {
                // Disable the script or perform any other action when sinking is complete
                enabled = false;
            }
        }
    }

    // Start the sinking process
    void StartSink()
    {
        timer = 1f; // Reset timer to start sinking
                    // Switch to camera2
        fallingTony.SetActive(false);
        audioSource.Play();
        audioSource2.Play();

        camera1.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);

}

// Calculate the distance the object needs to sink over the specified duration
private float CalculateSinkDistance()
    {
        return Vector3.Distance(startPosition, startPosition - Vector3.up * sinkDuration);
    }
}