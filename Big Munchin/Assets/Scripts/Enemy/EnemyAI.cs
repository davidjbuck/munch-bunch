/*Internal and external actions
 * External action:
 * Changing its healt
 * Raising a stat
 * Lowering a stat
 * Killing the AI
 * 
 * Internal actions:
 * Patrolling a path
 * Attacking a player
 * Fleeing from a player
 * Searching for a player
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//UPDATED CODE FOR SIMPLICY/READABILITY
public class EnemyAI : MonoBehaviour
{
    // Enum for different AI behaviors
    public enum Behaviors { Idle, Guard, Combat, Flee, Patrol };

    // AI properties and variables
    public Behaviors aiBehaviors = Behaviors.Idle;
    public bool dead = false;
    public bool isSuspicious = false;
    public bool isInRange = false;
    public bool FightsRanged = false;
    public float attackTimer;
    public bool aggressive;
    public bool permAggressive;
    public Rigidbody rigidBod;
    public float sprintSpeed;
    public float patrolSpeed;
    UnityEngine.AI.NavMeshAgent navAgent;
    Vector3 Destination;
    float Distance;
    public Transform[] Waypoints;
    int curWaypoint = 0;
    bool ReversePath = false;
    public PlayerMover p1;
    private float attackCooldown = 0;
    public float lightAttackCooldown;
    public float heavyAttackCooldown;
    public GameObject enemyAttack;
    public Transform attackSpawn;
    public Image lightAttackWarning;
    public Image heavyAttackWarning;

    private int randNum;
    private bool newRand;
    public MovesetHolder[] enemyMovesets;
    MovesetHolder enemyActiveMoveset;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables and objects
        enemyActiveMoveset = enemyMovesets[0];
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        Destination = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Randomly select an attack type
        if (!newRand)
        {
            randNum = Random.Range(0, 2);
            if (randNum == 0)
            {
                attackCooldown = lightAttackCooldown;
            }
            if (randNum == 1)
            {
                attackCooldown = heavyAttackCooldown;
            }
            newRand = true;
        }

        // Update attack timer
        if (attackTimer < (attackCooldown - 1))
        {
            attackTimer += Time.deltaTime;
        }

        // Perform AI behaviors if not dead
        if (dead == false)
        {
            RunBehaviors();
        }
    }

    // Execute different AI behaviors based on the current behavior state
    void RunBehaviors()
    {
        switch (aiBehaviors)
        {
            case Behaviors.Idle:
                Idle();
                break;
            case Behaviors.Guard:
                Guard();
                break;
            case Behaviors.Combat:
                Combat();
                break;
            case Behaviors.Flee:
                Flee();
                break;
			case Behaviors.Patrol:
				Flee();
				break;
		}
    }

    // Change the current AI behavior and execute corresponding actions
    void ChangeBehavior(Behaviors newBehavior)
    {
        aiBehaviors = newBehavior;
        RunBehaviors();
    }

    // Idle behavior - placeholder for animation or other actions
    void Idle()
    {
        if (!dead)
        {
            //GetComponent<Animation>().Play("dance");
        }
    }

    // Guard behavior - patrol or attack based on aggression and suspicion
    void Guard()
    {
        if (!dead)
        {
            Destination = player.transform.position;
            Distance = Vector3.Distance(transform.position, Destination);

            // Check if suspicious or permanently aggressive
            if (permAggressive || (isSuspicious && aggressive))
            {
                navAgent.speed = sprintSpeed;
                GuardSearchForTarget();
            }
            else
            {
                navAgent.speed = patrolSpeed;
                Patrol();
            }
        }
    }

    // Combat behavior - engage in combat with the player
    void Combat()
    {
        if (!dead)
        {
            if (Distance < 2)
            {
                Vector3 playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                transform.LookAt(playerPosition);
                Attack();
            }
            else if (Distance < 5)
            {
                navAgent.SetDestination(player.transform.position);
            }
            else
            {
                attackTimer = 200;
                SearchForTarget();
            }
        }
    }

    // Flee behavior - move away from the player
    void Flee()
    {
        if (!dead)
        {
            Destination = player.transform.position;
            Distance = Vector3.Distance(transform.position, Destination);
            if (Distance > 20f)
            {
                isSuspicious = false;
                Guard();
            }
            Destination = transform.position + (transform.position - player.transform.position);
            navAgent.SetDestination(Destination);
        }
    }

    // Guard searches for the player if within a certain distance
    void GuardSearchForTarget()
    {
        if (!dead && Distance < 40f)
        {
            if (Distance < 3f)
            {
                Combat();
            }
            else
            {
                ResetAttackWarnings();
            }
            navAgent.SetDestination(Destination);
        }
        else if (!permAggressive)
        {
            isSuspicious = false;
        }
    }

    // Search for the player within a certain distance
    void SearchForTarget()
    {
        if (!dead)
        {
            Destination = player.transform.position;
            Distance = Vector3.Distance(transform.position, Destination);
            if (Distance < 10f)
            {
                rigidBod.constraints = Distance < 2f ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
                navAgent.SetDestination(Destination);
            }
            else
            {
                rigidBod.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    // Patrol between waypoints
    void Patrol()
    {
        if (!dead && Waypoints.Length > 0)
        {
            Distance = Vector3.Distance(transform.position, Waypoints[curWaypoint].position);
            if (Distance > 2.00f)
            {
                Destination = Waypoints[curWaypoint].position;
                navAgent.SetDestination(Destination);
            }
            else
            {
                UpdateWaypoint();
            }
        }
    }

    // Perform attack actions
    void Attack()
    {
        if (attackTimer >= (attackCooldown - 1))
        {
            if (randNum == 0)
            {
                lightAttackWarning.enabled = true;
            }
            else if (randNum == 1)
            {
                heavyAttackWarning.enabled = true;
            }
            else
            {
                ResetAttackWarnings();
            }

            if (attackTimer <= attackCooldown)
            {
                attackTimer += Time.deltaTime;
            }

            if (attackTimer > attackCooldown)
            {
                ExecuteAttack();
            }

            Destination = transform.position;
            navAgent.SetDestination(Destination);
        }
    }

    // Execute the chosen attack type
    void ExecuteAttack()
    {
        if (randNum == 0)
        {
            enemyActiveMoveset.LightAttackCombo();
        }
        else if (randNum == 1)
        {
            enemyActiveMoveset.HeavyAttackCombo();
            attackCooldown -= 1;
        }

        newRand = false;
        attackTimer = 0;
    }

    // Reset attack warnings
    void ResetAttackWarnings()
    {
        lightAttackWarning.enabled = false;
        heavyAttackWarning.enabled = false;
    }

    // Update waypoint for patrolling
    void UpdateWaypoint()
    {
		if (ReversePath)
		{
			if (curWaypoint <= 0)
			{
				ReversePath = false;
			}
			else
			{
				curWaypoint--;
				Destination = Waypoints[curWaypoint].position;
			}
		}
		else
		{
			if (curWaypoint >= Waypoints.Length - 1)
			{
				ReversePath = true;
			}
			else
			{
				curWaypoint++;
				Destination = Waypoints[curWaypoint].position;
			}
		}

		Destination = Waypoints[curWaypoint].position;
    }
}


//ORIGINAL CODE FROM FIRST SEMESTER
/*

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

//public enum Behaviors { Idle, Guard, Combat, Flee };

public class EnemyAI : MonoBehaviour
{
	//public GameObject healthRect;
	//public Transform shotSpawn;
	//public Transform shotSpawn1;
	public Behaviors aiBehaviors = Behaviors.Idle;
	//public PlayerStats pstat;
	public bool dead = false;
	public bool isSuspicious = false;
	public bool isInRange = false;
	public bool FightsRanged = false;
	public float attackTimer;
	public bool aggressive;
	public bool permAggressive;
	//public GameObject Projectile;
	public Rigidbody rigidBod;
	public float sprintSpeed;
	public float patrolSpeed;
	UnityEngine.AI.NavMeshAgent navAgent;
	Vector3 Destination;
	float Distance;
	public Transform[] Waypoints;
	public int curWaypoint = 0;
	bool ReversePath = false;
	//Camera maincamera;
	//public int health = 100;
	//GameObject enemyObj;
	//public GameObject bones;
	public PlayerMover p1;
	//public GameObject enemy;
	//public GameObject healthText;
	private float attackCooldown = 0;
	public float lightAttackCooldown;
	public float heavyAttackCooldown;
	public GameObject enemyAttack;
	public Transform attackSpawn;
	public Image lightAttackWarning;
	public Image heavyAttackWarning;

	private int randNum;
	private bool newRand;
	public MovesetHolder[] enemyMovesets;
	MovesetHolder enemyActiveMoveset;
	GameObject player;

	#region Behaviors
	void RunBehaviors()
	{
		switch (aiBehaviors)
		{
			case Behaviors.Idle:
				RunIdleNode();
				break;
			case Behaviors.Guard:
				RunGuardNode();
				break;
			case Behaviors.Combat:
				RunCombatNode();
				break;
			case Behaviors.Flee:
				RunFleeNode();
				break;
		}
	}

	void ChangeBehavior(Behaviors newBehavior)
	{
		aiBehaviors = newBehavior;

		RunBehaviors();
	}

	void RunIdleNode()
	{
		Idle();
	}

	void RunGuardNode()
	{
		Guard();
	}

	void RunCombatNode()
	{
		Combat();
		//if (FightsRanged)
		//	RangedAttack();
		//else if (isInRange)
		//	MeleeAttack();
		//else
		//	SearchForTarget();
	}

	void RunFleeNode()
	{
		Flee();
	}
	#endregion

	#region Actions
	void Idle()
	{
		if (dead == false)
		{
			//GetComponent<Animation>().Play("dance");
		}
	}

	void Guard()
	{
		if (dead == false)
		{
			Destination = player.transform.position;
			Distance = Vector3.Distance(gameObject.transform.position, Destination);
			if (Distance < 20f)
			{
				isSuspicious = true;
			}
			if (permAggressive)
			{
				GuardSearchForTarget();
			}
			else if (isSuspicious)
			{
				//INCREASES SPEED TO EITHER ATTACK OR FLEE
				GetComponent<UnityEngine.AI.NavMeshAgent>().speed = sprintSpeed;
				//HERE
				//GUARDSERACH FOR TARGET MAKES AI LOOK FOR PLAYER IF THEY GET TOO CLOSE
				//FLEE MAKES AI RUN AWAY FROM PLAYER


				//THERE IS A TOGGLE FOR THIS, DEPENDING ON WHICH TYPE OF ANIMAL/ENEMY WE ARE USING
				if (aggressive)
				{
					GuardSearchForTarget();
				}
				else
				{
					Flee();
				}



			}
			else
			{
				GetComponent<UnityEngine.AI.NavMeshAgent>().speed = patrolSpeed;

				Patrol();
			}
		}
	}

	void Combat()
	{
		if (dead == false)
		{
			if (Distance < 2)
			{
				//attack player if they are within 5 unitys
				//attack timer and warnings
				Vector3 playerPosition = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
				this.transform.LookAt(playerPosition);
				//chooses random number 0 or 1 to decide which attack is being used (maybe something else in the future depending on health or stamina based)
				if (attackTimer >= (attackCooldown - 1) && randNum == 0)
				{
					//light attack warning
					lightAttackWarning.enabled = true;
					//Debug.Log("Light");
				}
				else if (attackTimer >= (attackCooldown - 1) && randNum == 1)
				{
					//heavy attack warning
					heavyAttackWarning.enabled = true;
					//Debug.Log("Heavy");
				}
				else
				{
					//disable attack warnings
					lightAttackWarning.enabled = false;
					heavyAttackWarning.enabled = false;
				}
				//continues attack timer since it preloads most of it normally, but needs to be close to the player to finish counting
				if (attackTimer <= (attackCooldown))
				{
					attackTimer += Time.deltaTime;
				}
				//attack when timer goes above he cooldown number
				if (attackTimer > attackCooldown)
				{
					//spawns hitbox that attacks
					//light
					if (randNum == 0)
					{
						enemyActiveMoveset.LightAttackCombo();
					}
					//heavy
					if (randNum == 1)
					{
						enemyActiveMoveset.HeavyAttackCombo();
						attackCooldown = attackCooldown - 1;
					}
					//old code for temp enemyhitbox
					//GameObject w = Instantiate(enemyAttack, attackSpawn.position, attackSpawn.rotation) as GameObject;
					//Destroy(w, 0.2f);

					//reset variables for next attack
					newRand = false;
					attackTimer = 0;
					//Debug.Log("ATTACK");
				}
				Destination = this.transform.position;
				navAgent.SetDestination(Destination);
			}
			else if (Distance < 5)
			{
				navAgent.SetDestination(player.transform.position);
			}
			else
			{
				attackTimer = 200;
				SearchForTarget();
			}
		}
	}

	void Flee()
	{
		if (dead == false)
		{
			Destination = player.transform.position;
			Distance = Vector3.Distance(gameObject.transform.position, Destination);
			if (Distance > 20f)
			{
				isSuspicious = false;
				Guard();
			}
			Destination = transform.position + (transform.position - player.transform.position);
			navAgent.SetDestination(Destination);
		}
	}
	void GuardSearchForTarget()
	{
		if (dead == false)
		{
			if (Distance < 40f)
			{
				if (Distance < 3f)
				{
					Combat();
				}
				else
				{
					lightAttackWarning.enabled = false;
					heavyAttackWarning.enabled = false;
				}
				navAgent.SetDestination(Destination);
			}
			else if (permAggressive)
			{
				navAgent.SetDestination(Destination);
			}
			else
			{
				isSuspicious = false;
			}
		}
	}
	void SearchForTarget()
	{
		if (dead == false)
		{
			Destination = player.transform.position;
			Distance = Vector3.Distance(gameObject.transform.position, Destination);
			if (Distance < 10f)
			{
				rigidBod.constraints = RigidbodyConstraints.None;


				navAgent.SetDestination(Destination);

			}
			else
			{
				rigidBod.constraints = RigidbodyConstraints.FreezeAll;


			}

		}
	}
	void Patrol()
	{
		if (dead == false)
		{
			if (Waypoints.Length > 0)
			{
				Distance = Vector3.Distance(gameObject.transform.position, Waypoints[curWaypoint].position);
				if (Distance > 2.00f)
				{
					Destination = Waypoints[curWaypoint].position;
					navAgent.SetDestination(Destination);
				}
				else
				{
					if (ReversePath)
					{
						if (curWaypoint <= 0)
						{
							ReversePath = false;
						}
						else
						{
							curWaypoint--;
							Destination = Waypoints[curWaypoint].position;
						}
					}
					else
					{
						if (curWaypoint >= Waypoints.Length - 1)
						{
							ReversePath = true;
						}
						else
						{
							curWaypoint++;
							Destination = Waypoints[curWaypoint].position;
						}
					}
				}
			}
		}
	}


	#endregion
	void OnCollisionEnter(Collision col)
	{
	}
	void death()
	{
		//GetComponent<Animation>().Play("die");
		//Destroy(this);
		//dead = true;
		//navAgent.SetDestination(this.transform.position);

		//navAgent.isStopped = true;
		//navAgent.Stop();
		//GetComponent<CapsuleCollider>().enabled = false;
		//rigidBod.constraints = RigidbodyConstraints.FreezeAll;

		//this.tag = null;
		
	}
	bool isDead()
	{
		return dead;
	}
	void Start()
	{
		enemyActiveMoveset = enemyMovesets[0];
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");
		Destination = player.transform.position;
		//	pstat = g1.GetComponent<PlayerStats>();
		//p1 = g1.GetComponent<PlayerMover>();
	}

	void Update()
	{
		if (!newRand)
		{
			randNum = Random.Range(0, 2);
			//makes attack cooldown depending on attack and what is put in the inpector
			if (randNum == 0)
			{
				attackCooldown = lightAttackCooldown;
				//Debug.Log("light cooldown");
			}
			if (randNum == 1)
			{
				attackCooldown = heavyAttackCooldown;
				//Debug.Log("heavy cooldown");

			}
			//makes sure it doesnt call both constantly, resets once the attack spawns
			newRand = true;
		}
		if (attackTimer < (attackCooldown - 1))
		{
			attackTimer += Time.deltaTime;
			//Debug.Log(attackTimer);
		}



		if (dead == false)
		{
			
			//healthRect.transform.localScale = new Vector3(health/100f, 0.1f, 0.1f);

			//rAttackTimer = rAttackTimer + 1;
			//mAttackTimer = mAttackTimer + 1;

			RunBehaviors();

		}
	}
}
*/