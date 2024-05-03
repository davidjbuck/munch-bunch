using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonyFalling : MonoBehaviour
{
    // Animator reference
    public Animator animator;

    // Force variable for backward movement
    public float backwardForce = 7f;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to the object
        animator = GetComponent<Animator>();

        // Start the falling animation after 2 seconds
        Invoke("StartFallingAnimation", 1.2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Start the falling animation and apply force after 2 seconds
    void StartFallingAnimation()
    {
        // Set the "falling" parameter to true in the Animator
        if (animator != null)
        {
        }
        audioSource.Play();

        // Apply a backward force to the object
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            animator.SetBool("Falling", true);

            rb.AddForce(transform.forward * backwardForce, ForceMode.Impulse); // Using transform.forward instead of -transform.forward
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found!");
        }
    }
}