using UnityEngine;
using System.Collections;

public class ThrowMeatball : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public GameObject meatballPrefab; // Prefab of the meatball to be thrown
    public float meatballCooldown;
    private float meatballTimer;
    public float meatballSpeed;
    public bool throwingMeatballs;
    public Animator animator;
    private Vector3 playerLocation;
    public Transform attackSpawn;
    private Vector3 playerPosition;
    private bool meatballThrown = false; // Flag to track whether the meatball has been thrown

    private void Start()
    {
        meatballTimer = meatballCooldown;
        // No need to start the coroutine here since it's inside ThrowMeatballs method
    }

    void ThrowMeatballs()
    {
        if (!meatballThrown) // Check if the meatball hasn't been thrown yet
        {
            animator.SetBool("Throw", true);
            StartCoroutine(ThrowDelayCoroutine());
            meatballThrown = true; // Set the flag to true to indicate that the meatball is being thrown
        }
    }

    IEnumerator ThrowDelayCoroutine()
    {
        yield return new WaitForSeconds(0.65f); // Wait for 0.75 seconds
        // Now you can throw the meatball

        // Calculate throw direction towards the player
        Vector3 throwDirection = (playerPosition - transform.position).normalized;

        // Add a random offset within the 10-degree window in each direction
        float randomAngle = Random.Range(0f, 0f);
        throwDirection = Quaternion.Euler(0f, randomAngle, 0f) * throwDirection;

        // Calculate the initial velocity for the meatball (horizontal and vertical)
        Vector3 initialVelocity = throwDirection * meatballSpeed;

        // Set the initial velocity of the meatball
        GameObject meatball = Instantiate(meatballPrefab, attackSpawn.position, Quaternion.identity);
        Rigidbody meatballRb = meatball.GetComponent<Rigidbody>();
        meatballRb.velocity = initialVelocity;
        Destroy(meatball, 3f);

        // Reset the meatball throw cooldown timer
        meatballTimer = meatballCooldown;

        // Switch back to Chase behavior after throwing the meatball
        animator.SetBool("Throw", false);

        meatballThrown = false; // Reset the flag once the coroutine ends
    }

    private void Update()
    {
        playerLocation = new Vector3(playerPosition.x, this.transform.position.y, playerPosition.z);
        this.transform.LookAt(playerLocation);
        playerPosition = player.transform.position;

        if (meatballTimer > 0)
        {
            meatballTimer -= Time.deltaTime;
        }
        else if (Vector3.Distance(transform.position, player.transform.position) <= 100)
        {
            playerPosition = player.transform.position;
            ThrowMeatballs();
        }
    }
}