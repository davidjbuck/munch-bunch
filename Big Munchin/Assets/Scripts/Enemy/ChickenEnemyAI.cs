using UnityEngine;

public class ChickenEnemyAI : MonoBehaviour
{
    public enum Behaviors { Idle, RandomWalk, Chase, Attack }

    public Behaviors aiBehavior = Behaviors.Idle;
    public Transform playerTransform;
    public float chaseRange = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    private float attackTimer = 0f;
    GameObject player;
    Vector3 playerPosition;
    public Animator animator;

    private UnityEngine.AI.NavMeshAgent navAgent;

    private bool isIdle = false;
    private float idleTimer = 0f;
    private float idleDuration = 0f;
    public Transform attackSpawn;
    //public GameObject birdAttack;
    //GameObject bAttack;
    public MovesetHolder[] enemyMovesets;
    MovesetHolder enemyActiveMoveset;
    public EnemyHealth eHealth;
    public GameObject chicken;


    void Start()
    {
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);
        animator.SetBool("Chasing", false);
        animator.SetBool("Attack", false);
        enemyActiveMoveset = enemyMovesets[0];

    }

    void Update()
    {
        if (!eHealth.enemyAlive)
        {
            death();
        }
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
            //Debug.Log(attackTimer);
            attackTimer -= Time.deltaTime;
        }
        if (aiBehavior == Behaviors.Attack)
        {
            Vector3 playerPosition = new Vector3(player.transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
            this.transform.LookAt(playerPosition);
            if (attackTimer <= 0)
            {
                startAttack();
            }
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
            navAgent.SetDestination(this.transform.position);

            aiBehavior = Behaviors.Attack;
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Chasing", false);
            animator.SetBool("Attack", true);
        }
    }

    void Attack()
    {
        //        startAttack();
        Debug.Log("ATTACKING");
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
            attackTimer -= Time.deltaTime;
            return;
        }

        // If player is within attack range, attack
        if (Vector3.Distance(transform.position, playerPosition) <= attackRange)
        {
            // Perform attack action here

        }
    }
    public void startAttack()
    {
        //bAttack = Instantiate(birdAttack, attackSpawn.transform.position, attackSpawn.transform.rotation);
        //Destroy(bAttack, 0.1f);
        enemyActiveMoveset.LightAttackCombo();

        attackTimer = attackCooldown;
        Debug.Log("Attacking player!");
    }
    public void death()
    {
        Destroy(chicken.gameObject);
         
    }
}