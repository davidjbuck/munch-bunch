using UnityEngine;

public class RunningBird : MonoBehaviour
{
    public enum Behaviors { Idle, RandomWalk, Chase, Attack }

    public Behaviors aiBehavior = Behaviors.Idle;
    public Transform playerTransform;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private float attackTimer = 0f;
    GameObject player;
    Vector3 playerPosition;
    public Animator animator;

    private UnityEngine.AI.NavMeshAgent navAgent;

    private bool isIdle = false;
    private float idleTimer = 0f;
    private float idleDuration = 0f;

    void Start()
    {
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);
        animator.SetBool("Chasing", false);
        animator.SetBool("Attack", false);
    }

    void Update()
    {
        // Update player position
        playerPosition = player.transform.position;

        // Check if player is within chase range
        if (Vector3.Distance(transform.position, playerPosition) <= chaseRange)
        {
            // If player is within chase range, switch to chase behavior
            aiBehavior = Behaviors.Chase;
        }

        // Perform behavior based on current state
        switch (aiBehavior)
        {
            case Behaviors.Idle:
                Idle();
                break;
            case Behaviors.RandomWalk:
                RandomWalk();
                break;
            case Behaviors.Chase:
                Chase();
                break;
            case Behaviors.Attack:
                Attack();
                break;
        }

        // Update attack cooldown
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void Idle()
    {

        // Check if player is within chase range
        if (Vector3.Distance(transform.position, playerPosition) <= chaseRange)
        {
            // If player is within chase range, switch to chase behavior
            aiBehavior = Behaviors.Chase;
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Chasing", true);
            animator.SetBool("Attack", false);
            return;
        }

        // Check if currently idling
        if (isIdle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0)
            {
                // Idle time elapsed, switch to random walk behavior
                isIdle = false;
                navAgent.isStopped = false;
                aiBehavior = Behaviors.RandomWalk;
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", true);
                animator.SetBool("Chasing", false);
                animator.SetBool("Attack", false);
                return;
            }
            else
            {
                // Still idling
                return;
            }
        }

        // Start idling
        StartIdle();
    }

    void RandomWalk()
    {


        idleTimer = 0;
        navAgent.isStopped = false;
        isIdle = false;
        // Move randomly within the NavMesh area
        if (!navAgent.hasPath || navAgent.remainingDistance < 1f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            UnityEngine.AI.NavMeshHit hit;
            UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 10f, UnityEngine.AI.NavMesh.AllAreas);
            Vector3 finalPosition = hit.position;
            navAgent.SetDestination(finalPosition);
            if (Random.Range(0, 6) == 0) // 1 in 4 chance
            {
                aiBehavior = Behaviors.Idle;
                animator.SetBool("Idle", true);
                animator.SetBool("Walk", false);
                animator.SetBool("Chasing", false);
                animator.SetBool("Attack", false);
                StartIdle();
            }
        }
    }

    void StartIdle()
    {
        isIdle = true;
        navAgent.isStopped = true;
        idleDuration = Random.Range(5f, 10f);
        idleTimer = idleDuration;
    }
    void Chase()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Chasing", true);
        animator.SetBool("Attack", false);
        idleTimer = 0;
        navAgent.isStopped = false;
        isIdle = false;
        // Check if player is out of chase range
        if (Vector3.Distance(transform.position, playerPosition) > chaseRange)
        {
            // If player is out of range, switch to idle behavior
            aiBehavior = Behaviors.Idle;
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Chasing", false);
            animator.SetBool("Attack", false);
            return;
        }

        // Chase the player
        navAgent.SetDestination(playerPosition);

        // If within attack range, switch to attack behavior
        if (Vector3.Distance(transform.position, playerPosition) <= attackRange)
        {
            aiBehavior = Behaviors.Attack;
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Chasing", false);
            animator.SetBool("Attack", true);
        }
    }

    void Attack()
    {
        // If player is out of range, switch to chase behavior
        if (Vector3.Distance(transform.position, playerPosition) > chaseRange)
        {
            aiBehavior = Behaviors.Chase;
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Chasing", true);
            animator.SetBool("Attack", false);
            return;
        }

        // If attack is on cooldown, return
        if (attackTimer > 0)
        {
            return;
        }

        // Attack the player
        // Replace this with your attack logic
        Debug.Log("Attacking player!");

        // Reset attack cooldown
        attackTimer = attackCooldown;
    }
}