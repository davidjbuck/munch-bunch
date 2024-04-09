using UnityEngine;

public class TonyEnemy : MonoBehaviour
{
    public enum Behaviors { Idle, MoveAway, Stunned, Chase, Attack, ThrowMeatball, Charge }

    public Behaviors aiBehavior = Behaviors.Idle;
    public Transform playerTransform;
    public float moveAwayDistance = 10f;
    public float stunDuration = 3f;
    public float chaseRange = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    public float chargeSpeed = 10f;
    public float normalSpeed = 5f;
    private Vector3 playerPosition;
    private Vector3 tempPlayerPosition;
    private bool charging;
    private float stunTimer = 0f;
    private float attackTimer = 0f;
    public float meatballCooldown;
    private float meatballTimer;
    public Transform attackSpawn;
    public GameObject meatballPrefab;
    private UnityEngine.AI.NavMeshAgent navAgent;
    public Animator animator;
    public float meatballSpeed;
    void Start()
    {
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerPosition = playerTransform.position;
        charging = false;
    }

    void Update()
    {
        // Update player position
        playerPosition = playerTransform.position;

        // Perform behavior based on current state
        switch (aiBehavior)
        {
            case Behaviors.Idle:
                Idle();
                break;
            case Behaviors.MoveAway:
                MoveAway();
                break;
            case Behaviors.Stunned:
                Stunned();
                break;
            case Behaviors.Chase:
                Chase();
                break;
            case Behaviors.Attack:
                Attack();
                break;
            case Behaviors.ThrowMeatball:
                ThrowMeatball();
                break;
            case Behaviors.Charge:
                Charge();
                break;
        }

        // Update attack cooldown
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        if(meatballTimer > 0)
        {
            meatballTimer -= Time.deltaTime;
        }
    }

    void SetAnimatorBools()
    {
        navAgent.speed = normalSpeed;

        // Set animator state bools based on current behavior
        animator.SetBool("Idle", aiBehavior == Behaviors.Idle);
        animator.SetBool("MoveAway", aiBehavior == Behaviors.MoveAway);
        animator.SetBool("Stunned", aiBehavior == Behaviors.Stunned);
        animator.SetBool("Chase", aiBehavior == Behaviors.Chase);
        animator.SetBool("Attack", aiBehavior == Behaviors.Attack);
        animator.SetBool("ThrowMeatball", aiBehavior == Behaviors.ThrowMeatball);
        animator.SetBool("Charge", aiBehavior == Behaviors.Charge);
    }

    void Idle()
    {
        // Check conditions to switch behavior
        if (Vector3.Distance(transform.position, playerPosition) <= moveAwayDistance)
        {
            // If player is too close, switch to MoveAway behavior
            aiBehavior = Behaviors.MoveAway;
            SetAnimatorBools();
            return;
        }

        // Check if player is within chase range
        if (Vector3.Distance(transform.position, playerPosition) <= chaseRange)
        {
            // If player is within chase range, switch to Chase behavior
            aiBehavior = Behaviors.Chase;
            SetAnimatorBools();
            return;
        }

        // Perform idle behavior here
    }

    void MoveAway()
    {
        // Move away from the player
        Vector3 Destination = playerPosition;
        float Distance = Vector3.Distance(transform.position, Destination);
        Destination = transform.position + (transform.position - playerPosition);
        navAgent.SetDestination(Destination);
        // Check conditions to switch behavior
        if (Vector3.Distance(transform.position, playerPosition) > moveAwayDistance)
        {
            // If player is far enough, switch back to Idle behavior
            aiBehavior = Behaviors.Idle;
            SetAnimatorBools();
            return;
        }
    }

    void Stunned()
    {
        // Handle stunned behavior here
        if (stunTimer <= 0)
        {
            // Stun duration elapsed, switch back to Idle behavior
            aiBehavior = Behaviors.Idle;
            SetAnimatorBools();
            return;
        }

        // Decrement stun timer
        stunTimer -= Time.deltaTime;
        Debug.Log("STUN TIMER: " + stunTimer);
    }

    void Chase()
    {
        // Check if player is out of chase range
        if (Vector3.Distance(transform.position, playerPosition) > chaseRange)
        {
            // If player is out of range, switch to Idle behavior
            aiBehavior = Behaviors.Idle;
            SetAnimatorBools();
            return;
        }

        // Chase the player
        navAgent.SetDestination(playerPosition);

        // If within attack range, switch to Attack behavior
        if (Vector3.Distance(transform.position, playerPosition) <= attackRange)
        {
            aiBehavior = Behaviors.Attack;
            SetAnimatorBools();
            return;
        }
    }

    void Attack()
    {
        // If attack is on cooldown, return
        if (attackTimer > 0)
        {
            return;
        }

        // If player is out of range, switch to Chase behavior
        if (Vector3.Distance(transform.position, playerPosition) > attackRange)
        {
            aiBehavior = Behaviors.Chase;
            SetAnimatorBools();
            return;
        }

        // Decide between charging and throwing meatball
        if (Random.Range(0, 2) == 0)
        {
            // 50% chance to charge
            //aiBehavior = Behaviors.ThrowMeatball;
            aiBehavior = Behaviors.Charge;
            SetAnimatorBools();
        }
        else
        {
          //  aiBehavior = Behaviors.Charge;

            // 50% chance to throw meatball
             aiBehavior = Behaviors.ThrowMeatball;

            SetAnimatorBools();

        }

        // Start attack cooldown
        attackTimer = attackCooldown;
    }

    void ThrowMeatball()
    {
        Vector3 lookPlayerPosition = new Vector3(playerTransform.position.x, this.transform.position.y, playerTransform.position.z);
        this.transform.LookAt(lookPlayerPosition);
        navAgent.SetDestination(transform.position);
        if (meatballTimer <= 0)
        {
            // Calculate throw direction towards the player
            Vector3 throwDirection = (playerPosition - transform.position).normalized;

            // Add a random offset within the 10-degree window in each direction
            float randomAngle = Random.Range(-10f, 10f);
            throwDirection = Quaternion.Euler(0f, randomAngle, 0f) * throwDirection;

            // Calculate the initial velocity for the meatball (horizontal and vertical)
            Vector3 initialVelocity = throwDirection * meatballSpeed;

            // Set the initial velocity of the meatball
            GameObject meatball = Instantiate(meatballPrefab, attackSpawn.position, Quaternion.identity);
            Rigidbody meatballRb = meatball.GetComponent<Rigidbody>();
            meatballRb.velocity = initialVelocity;

            // Reset the meatball throw cooldown timer
            meatballTimer = meatballCooldown;
        }
        else
        {
            // Meatball is on cooldown, do nothing
            return;
        }

        // Check if the meatball has reached the player
        if (Vector3.Distance(transform.position, playerPosition) <= 4f)
        {
            // Switch back to Idle behavior after throwing the meatball
            aiBehavior = Behaviors.Idle;
            SetAnimatorBools();
        }
    }

    void Charge()
    {
        navAgent.speed = chargeSpeed;

        if (!charging)
        {
            Debug.Log("CHARGING");
            Debug.Log(playerPosition);
            tempPlayerPosition = playerPosition;
            // Move towards the player's position
            navAgent.SetDestination(tempPlayerPosition);
            charging = true;
        }

        // Check if reached player's position
        if (Vector3.Distance(transform.position, tempPlayerPosition) <= 1f)
        {
            navAgent.speed = normalSpeed;

            Debug.Log(Vector3.Distance(transform.position, tempPlayerPosition) + "DISTANCE");
            charging = false;
            // Apply stun once reached
            ApplyStun();
        }
    }

    // Method to apply stun effect
    public void ApplyStun()
    {
        navAgent.speed = normalSpeed;
        aiBehavior = Behaviors.Stunned;
        stunTimer = stunDuration;
        SetAnimatorBools();
    }
    void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.tag == "Hitbox" && col.GetComponent<CollisionManager>().GetAttackTeam() == 0)
        {
            navAgent.speed = normalSpeed;

            Debug.Log("TONY HIT");
            aiBehavior = Behaviors.MoveAway;
        }
    }
}