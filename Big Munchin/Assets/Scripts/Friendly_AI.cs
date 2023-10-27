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

//public enum Behaviors { Idle, Guard, Combat, Flee };

public class Friendly_AI : MonoBehaviour
{
	//public GameObject healthRect;
	//public Transform shotSpawn;
	//public Transform shotSpawn1;
	public Behaviors aiBehaviors = Behaviors.Idle;
	//public PlayerStats pstat;
	/*
	public bool dead = false;
	public bool isSuspicious = false;
	public bool isInRange = false;
	public bool FightsRanged = false;
	public float attackTimer;
	public bool aggressive;
	public bool permAggressive;
	*/
	//public GameObject Projectile;
	public bool nearPlayer;
	public bool interacting;
	public Rigidbody rigidBod;
	public float sprintSpeed;
	public float patrolSpeed;
	UnityEngine.AI.NavMeshAgent navAgent;
	Vector3 Destination;
	Vector3 playerDestination;
	float Distance;
	float playerDistance;
	public Transform[] Waypoints;
	public int curWaypoint = 0;
	bool ReversePath = false;
	public TMP_Text allyInteract;
	//Camera maincamera;
	//public int health = 100;
	//GameObject enemyObj;
	//public GameObject bones;
	public PlayerMover p1;
	//public GameObject enemy;
	//public GameObject healthText;
	/*
	private float attackCooldown = 0;
	public float lightAttackCooldown;
	public float heavyAttackCooldown;
	public GameObject enemyAttack;
	public Transform attackSpawn;
	public Image lightAttackWarning;
	public Image heavyAttackWarning;
	*/
	//private int randNum;
	//private bool newRand;
	//public MovesetHolder[] enemyMovesets;
	//MovesetHolder enemyActiveMoveset;
	#region Behaviors
	/*
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

	}
	
	void Guard()
	{
		if (dead == false)
		{
			Destination = GameObject.FindGameObjectWithTag("Player").transform.position;
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

	


	void GuardSearchForTarget()
	{
		if (dead == false)
		{
			if (Distance < 40f)
			{
				if (Distance < 5f)
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
			Destination = GameObject.FindGameObjectWithTag("Player").transform.position;
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
	*/
	void Patrol()
	{
		playerDestination = GameObject.FindGameObjectWithTag("Player").transform.position;
		playerDistance = Vector3.Distance(gameObject.transform.position, playerDestination);
		//playerDistance = Vector3.Distance(gameObject.transform.position, this.transform.position);
		if(playerDistance < 5f)
        {
			nearPlayer = true;
			Vector3 playerPosition = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
			allyInteract.enabled = true;

			this.transform.LookAt(playerPosition);
			Destination = this.transform.position;
			navAgent.SetDestination(Destination);
		} else
        {
			allyInteract.enabled = false;
			nearPlayer = false;
        }
		Distance = Vector3.Distance(gameObject.transform.position, Waypoints[curWaypoint].position);
		if (nearPlayer == false) {
			if (Distance > 5.00f)
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


	void Start()
	{
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //GameObject g1 = GameObject.FindGameObjectWithTag("Player");
        //	pstat = g1.GetComponent<PlayerStats>();
        //p1 = g1.GetComponent<PlayerMover>();
	}

	void Update()
	{
		Patrol();
	}
}