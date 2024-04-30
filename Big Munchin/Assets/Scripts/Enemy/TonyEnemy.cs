using UnityEngine;
public class TonyEnemy : MonoBehaviour
{
    // Enum for different behaviors
    public enum Behaviors { Idle, MoveAway, Stunned, Chase, Attack, ThrowMeatball, Charge, MeleeAttack, Punch, RunAway }

    // Public variables
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
    public bool throwingMeatballs;
    private float moveAwayTimer = 0f; // Timer for MoveAway behavior
    private int hitCounter;
    private Vector3 playerLocation;
    public MovesetHolder[] enemyMovesets;
    MovesetHolder enemyActiveMoveset;
    public EnemyHealth eHealth;
    public Transform elevator;
    private bool dead;
    public float punchCooldown = 1f;
    private float punchTimer = 0f;
    public GameObject tonyElevator;
    public GameObject elevatorObj;
    void Start()
    {
        // Initialize components and variables
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //animator = GetComponent<Animator>();
        playerPosition = playerTransform.position;
        charging = false;
        hitCounter = 0;
        enemyActiveMoveset = enemyMovesets[0];
       // animator.SetBool("Chase", true);

        //SetAnimatorBools();
    }

    void Update()
    {
        if (!eHealth.enemyAlive)
        {
            death();
            dead = true;
        }
        // Update player position
        playerPosition = playerTransform.position;

        // Perform behavior based on current state
        if (punchTimer > 0)
        {
            punchTimer -= Time.deltaTime;
        }
        if (!dead)
        {
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
                case Behaviors.Charge:
                    Charge();
                    break;
                case Behaviors.MeleeAttack:
                    MeleeAttack();
                    break;
                case Behaviors.Punch:
                    Punch();
                    break;
                case Behaviors.RunAway:
                    RunAway();
                    break;
            }
        } else
        {
        
                    RunAway();

        }
        

        // Update timers
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        if (meatballTimer > 0)
        {
            meatballTimer -= Time.deltaTime;
        }
        if (moveAwayTimer > 0)
        {
            moveAwayTimer -= Time.deltaTime;

            // If the MoveAway timer expires, switch back to Idle behavior
            if (moveAwayTimer <= 0)
            {
                aiBehavior = Behaviors.Idle;
                SetAnimatorBools();
            }
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
        animator.SetBool("MeleeAttack", aiBehavior == Behaviors.MeleeAttack);
        animator.SetBool("RunAway", aiBehavior == Behaviors.RunAway);

         animator.SetBool("Punch", aiBehavior == Behaviors.Punch);

        hitCounter = 0;
        stunTimer = stunDuration;
    }

    void Idle()
    {
        // Check conditions to switch behavior
        if (Vector3.Distance(transform.position, playerPosition) <= moveAwayDistance)
        {
            // If player is too close, switch to MoveAway behavior
            aiBehavior = Behaviors.MoveAway;
            SetAnimatorBools();
            moveAwayTimer = 3f; // Start the MoveAway timer
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
        Vector3 Destination = transform.position + (transform.position - playerPosition);
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
            if (Vector3.Distance(transform.position, playerPosition) < 5)
            {
                aiBehavior = Behaviors.MeleeAttack;
                SetAnimatorBools();
            } else
            {
                aiBehavior = Behaviors.Idle;
                SetAnimatorBools();
            }


            return;
        }

        // Decrement stun timer
        stunTimer -= Time.deltaTime;
       // Debug.Log("STUN TIMER: " + stunTimer);
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
            navAgent.SetDestination(this.transform.position);

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

        if (Vector3.Distance(transform.position, playerPosition) < 4)
        {
            //MELEE COMBAT

            aiBehavior = Behaviors.MeleeAttack;
            SetAnimatorBools();
        }
        if (Vector3.Distance(transform.position, playerPosition) < attackRange && Vector3.Distance(transform.position, playerPosition) > 4)
        {
            attackTimer = attackCooldown;

            //aiBehavior = Behaviors.ThrowMeatball;

            aiBehavior = Behaviors.Charge;
            SetAnimatorBools();
        }

        // Start attack cooldown
        attackTimer = attackCooldown;
    }
    void Punch()
    {
        Debug.Log("PUNCH");
        //attacks here
        enemyActiveMoveset.LightAttackCombo();

        playerLocation = new Vector3(playerPosition.x, this.transform.position.y, playerPosition.z);
        this.transform.LookAt(playerLocation);
        aiBehavior = Behaviors.Attack;
        SetAnimatorBools();
    }
    void MeleeAttack()
    {
        Debug.Log("MELEE ATTACK");
        if (Vector3.Distance(transform.position, playerPosition) < 2f)
        {
            navAgent.SetDestination(this.transform.position);

            // Check if punch is off cooldown
            if (punchTimer <= 0)
            {
                // If punch is off cooldown, execute punch
                aiBehavior = Behaviors.Punch;
                SetAnimatorBools();

                // Reset punch cooldown timer
                punchTimer = punchCooldown;
            }
        }
        else if (Vector3.Distance(transform.position, playerPosition) >= 2f)
        {
            navAgent.SetDestination(playerPosition);
        }
    }
    void RunAway()
    {
        navAgent.speed = 10f;
        
        this.transform.LookAt(new Vector3(elevator.position.x, this.transform.position.y, elevator.position.z));
        navAgent.SetDestination(elevator.position);
        if (Vector3.Distance(transform.position, navAgent.destination) <= 2f)
        {
            tonyElevator.SetActive(true);  
            elevatorObj.GetComponent<ElevatorMoveUp>().startElevator();
            Destroy(gameObject);
        }
    }
    void death()
    {
        aiBehavior = Behaviors.RunAway;
        SetAnimatorBools();
    }

    void Charge()
    {
        stunTimer = stunDuration;

        navAgent.speed = chargeSpeed;

        if (!charging)
        {
            Debug.Log("CHARGING");
            Vector3 chargeDirection = (playerPosition - transform.position).normalized;
            Vector3 chargeDestination = transform.position + chargeDirection * (Vector3.Distance(transform.position, playerPosition) + 0.75f);
            navAgent.SetDestination(chargeDestination);
            charging = true;
        }

        // Check if reached destination
        if (Vector3.Distance(transform.position, navAgent.destination) <= 1f)
        {
            navAgent.speed = normalSpeed;
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
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Hitbox" && col.GetComponent<CollisionManager>().GetAttackTeam() == 0 && aiBehavior != Behaviors.Stunned)
        {
            if (charging)
            {
                ApplyStun();
            }
            hitCounter++;
            if (hitCounter >= 5)
            {
                navAgent.speed = normalSpeed;
                aiBehavior = Behaviors.MoveAway;
                SetAnimatorBools();

            }
        }
        if(col.gameObject.tag == "Player" && charging)
        {
            Debug.Log("HIT WHILE CHARGING");
            //spawn hitbox
            enemyActiveMoveset.HeavyAttackCombo();
        }
    }
}