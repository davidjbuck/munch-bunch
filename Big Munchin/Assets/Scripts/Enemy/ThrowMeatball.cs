using UnityEngine;

public class ThrowMeatball : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public GameObject meatballPrefab; // Prefab of the meatball to be thrown
    public float throwForce = 10f; // Force to throw the meatball
    public float arcHeight = 2f; // Height of the arc

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player transform not assigned!");
            enabled = false; // Disable the script if player transform is not assigned
        } else
        {
            ThrowMeatballAttack();

        }
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
        }
        */
    }

    private void ThrowMeatballAttack()
    {
        // Calculate the direction towards the player
        Vector3 targetPosition = player.position;
        Vector3 throwDirection = (targetPosition - transform.position).normalized;

        // Adjust the throwing direction based on the player's position relative to the starting point
        Vector3 adjustedThrowDirection = Quaternion.AngleAxis(Vector3.Angle(transform.forward, throwDirection), Vector3.up) * transform.forward;

        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, targetPosition);

        // Calculate the initial velocity needed to reach the target position
        float initialVelocity = Mathf.Sqrt(distanceToPlayer * Physics.gravity.magnitude / Mathf.Sin(2 * Mathf.Deg2Rad * 45));

        // Apply arc height to the throw direction
        Vector3 throwArc = adjustedThrowDirection + Vector3.up * arcHeight;

        // Instantiate the meatball prefab
        GameObject meatball = Instantiate(meatballPrefab, transform.position, Quaternion.identity);

        // Get the Rigidbody component of the meatball
        Rigidbody rb = meatball.GetComponent<Rigidbody>();

        // Set the velocity of the meatball
        rb.velocity = throwArc * initialVelocity;

        // Apply additional force to account for the initial throw force
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }
}