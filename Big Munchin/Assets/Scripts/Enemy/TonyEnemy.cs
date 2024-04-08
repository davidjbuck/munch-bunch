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
    public float chargeSpeed = 5f;

    private Vector3 playerPosition;
    private Vector3 tempPlayerPosition;
    private bool charging;
    private float stunTimer = 0f;
    private float attackTimer = 0f;

    private UnityEngine.AI.NavMeshAgent navAgent;
    public Animator animator;

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
    }

    void SetAnimatorBools()
    {
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
        Vector3 direction = transform.position - playerPosition;
        direction.y = 0; // Ensure movement is on the XZ plane
        direction.Normalize();
        Vector3 targetPosition = transform.position + direction * moveAwayDistance;
        navAgent.SetDestination(targetPosition);

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
            aiBehavior = Behaviors.Charge;
            SetAnimatorBools();
        }
        else
        {
            aiBehavior = Behaviors.Charge;
            SetAnimatorBools();

            // 50% chance to throw meatball
            // aiBehavior = Behaviors.ThrowMeatball;
        }

        // Start attack cooldown
        attackTimer = attackCooldown;
    }

    void ThrowMeatball()
    {
        // Implement throwMeatball behavior here
    }

    void Charge()
    {

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
            Debug.Log(Vector3.Distance(transform.position, tempPlayerPosition) + "DISTANCE");
            charging = false;
            // Apply stun once reached
            ApplyStun();
        }
    }

    // Method to apply stun effect
    public void ApplyStun()
    {
        aiBehavior = Behaviors.Stunned;
        stunTimer = stunDuration;
        SetAnimatorBools();
    }
}