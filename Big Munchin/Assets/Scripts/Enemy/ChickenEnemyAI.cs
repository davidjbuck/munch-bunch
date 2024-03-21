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
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


public class ChickenEnemyAI : MonoBehaviour
{
	public enum Behaviors { Idle, Guard, Combat, Flee };

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
	bool aggressive = true;
	bool permAggressive = true;
	//public GameObject Projectile;
	public Rigidbody rigidBod;
	public float sprintSpeed;
	public float patrolSpeed;
	UnityEngine.AI.NavMeshAgent navAgent;
	Vector3 Destination;
	float Distance;
	//public Transform[] Waypoints;
	public int curWaypoint = 0;
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
	public bool wave1RunningEnemies;
	private int randNum;
	private bool newRand;
	public MovesetHolder[] enemyMovesets;
	MovesetHolder enemyActiveMoveset;
	GameObject player;
	private bool movingToRestaurant;
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
	void RunFleeToRestaurantNode()
	{
		FleeToRestaurant();
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
				Vector3 playerPosition = new Vector3(player.transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
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

	public void Flee()
	{
		if (dead == false)
		{
			Destination = player.transform.position;
			Distance = Vector3.Distance(gameObject.transform.position, Destination);
			if (Distance > 10f)
			{
				isSuspicious = false;
				Guard();
			}
			Destination = transform.position + (transform.position - player.transform.position);
			navAgent.SetDestination(Destination);
		}
	}
	public void FleeToRestaurant()
	{
		if (dead == false)
		{
			Destination = GameObject.FindGameObjectWithTag("restaurantsidedoor").transform.position;
			Debug.Log(Destination);
			navAgent.SetDestination(Destination);
			Distance = Vector3.Distance(transform.position, Destination);
			//Debug.Log(Distance);
			movingToRestaurant = true;

		}
	}
	void GuardSearchForTarget()
	{
		if (dead == false)
		{
			if (Distance < 40f)
			{
				if (Distance < 1.5f)
				{
					Combat();
					RunCombatNode();
					Destination = this.transform.position;
					navAgent.SetDestination(Destination);

				}
				else
				{
					lightAttackWarning.enabled = false;
					heavyAttackWarning.enabled = false;
					Destination = player.transform.position;
					navAgent.SetDestination(Destination);

				}
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
		GuardSearchForTarget();
	}

	/*
	void RangedAttack()
	{
	}
	void MeleeAttack()
	{
	}
	*/
	#endregion


	/*
	void removeHealth(int rHealth)
	{
		health = health - rHealth;
	}
	*/
	void OnCollisionEnter(Collision col)
	{
	}
	void death()
	{
		/*
		//GetComponent<Animation>().Play("die");
		Destroy(this);
		dead = true;
		navAgent.SetDestination(this.transform.position);

		navAgent.isStopped = true;
		//navAgent.Stop();
		//GetComponent<CapsuleCollider>().enabled = false;
		rigidBod.constraints = RigidbodyConstraints.FreezeAll;

		//this.tag = null;
		*/
	}
	bool isDead()
	{
		return dead;
	}
	void Start()
	{
		wave1RunningEnemies = false;
		enemyActiveMoveset = enemyMovesets[0];
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");
		Destination = player.transform.position;
		//	pstat = g1.GetComponent<PlayerStats>();
		//p1 = g1.GetComponent<PlayerMover>();
	}

	void Update()
	{
		//Debug.Log(aiBehaviors);
		if (!newRand)
		{
			randNum = 0;
			attackCooldown = lightAttackCooldown;

			newRand = true;
		}
		
		if (attackTimer < (attackCooldown - 1))
		{
			attackTimer += Time.deltaTime;
			//Debug.Log(attackTimer);
		}



		if (dead == false)
		{
			/*
			//healthRect.transform.localScale = new Vector3(health/100f, 0.1f, 0.1f);

			rAttackTimer = rAttackTimer + 1;
			mAttackTimer = mAttackTimer + 1;
			*/
			if (movingToRestaurant)
			{
				//Destination = GameObject.FindGameObjectWithTag("restaurantsidedoor").transform.position;
				//Debug.Log(Destination);
				//navAgent.SetDestination(Destination);
				Distance = Vector3.Distance(transform.position, Destination);
				//Debug.Log(Distance);
				if (Distance < 1.00f)
				{
					Destroy(this.gameObject);
					//Debug.Log("DELETE HERE");
					//Destroy(this.GameObject);
				}
			}
			if (wave1RunningEnemies)
			{
				Flee();
			}
			RunBehaviors();

		}
	}
}