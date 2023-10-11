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

public class AnimalAI : MonoBehaviour
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
	public int attackTimer;
	public bool aggressive;
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


	public GameObject enemyAttack;
	public Transform attackSpawn;
	public Image attackWarning;

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
			Destination = GameObject.FindGameObjectWithTag("Player").transform.position;
			Distance = Vector3.Distance(gameObject.transform.position, Destination);
			if (Distance < 20f)
			{
				isSuspicious = true;
			}
			if (isSuspicious)
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
			if (Distance < 5)
			{
				//navAgent.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
				Vector3 playerPosition = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
				this.transform.LookAt(playerPosition);
				if(attackTimer > 300)
                {
					attackWarning.enabled = true;
                } else
                {
					attackWarning.enabled = false;
				}
				if (attackTimer > 500)
				{
					GameObject w = Instantiate(enemyAttack, attackSpawn.position, attackSpawn.rotation) as GameObject;
					Destroy(w, 0.2f);

					attackTimer = 0;
					Debug.Log("ATTACK");
				}
				Destination = this.transform.position;
				navAgent.SetDestination(Destination);
			}
			else
			{
				SearchForTarget();
			}
		}
	}

	void Flee()
	{
		if (dead == false)
		{
			Destination = GameObject.FindGameObjectWithTag("Player").transform.position;
			Distance = Vector3.Distance(gameObject.transform.position, Destination);
			if (Distance > 20f)
			{
				isSuspicious = false;
				Guard();
			}
			Destination = transform.position + (transform.position - GameObject.FindGameObjectWithTag("Player").transform.position);
			navAgent.SetDestination(Destination);

			//GetComponent<Animation>().Play("run");
			/*
			for (int fleePoint = 0; fleePoint < Waypoints.Length; fleePoint++)
			{
				Distance = Vector3.Distance(gameObject.transform.position, Waypoints[fleePoint].position);
				if (Distance > 20.00f)
				{
					Destination = Waypoints[curWaypoint].position;
					navAgent.SetDestination(Destination);
					break;
				}
				else if (Distance < 2.00f)
				{

					//ChangeBehavior(Behaviors.Idle);
				}
			}
			*/

		}
	}
	void GuardSearchForTarget()
	{
		if (dead == false)
		{
			if (Distance < 40f)
			{
				if(Distance < 5f)
                {
					Combat();
                }
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

				//GetComponent<Animation>().Play("dance");

			}
			/*
			if (Distance < 5.00f)
			{
				isInRange = true;
				FightsRanged = true;
			} else
			{
				FightsRanged = false;
			}4
			*/
			/*
			Destination = GameObject.FindGameObjectWithTag("Player").transform.position;
			Distance = Vector3.Distance(gameObject.transform.position, Destination);
			if (Distance < 10.00f)
			{
				GetComponent<Animation>().Play("run");

				navAgent.SetDestination(Destination);

				if (Distance < 5.00f)
				{
					//ChangeBehavior(Behaviors.Combat);
					isInRange = true;
					FightsRanged = true;
				} else
				{
					isInRange = false;
					FightsRanged = false;
					//SearchForTarget();
				}
			}
			else
			{
				//ChangeBehavior(Behaviors.Idle);
				navAgent.SetDestination(this.transform.position);
				Idle();
				isInRange = false;
				FightsRanged = false;
			}
			*/



			/*
			Distance = Vector3.Distance(gameObject.transform.position, Destination);
			Destination = GameObject.FindGameObjectWithTag("Player").transform.position;

			if (Distance < 10)
			{
				//isInRange = false;
				if (Distance < 5)
				{
					isInRange = true;
					if (Distance < 2)
					{
						FightsRanged = false;

					}
					else
					{
						FightsRanged = true;
					}
				} else
				{
					FightsRanged = false;
					navAgent.SetDestination(Destination);
					GetComponent<Animation>().Play("run");
				}
			}
			if(Distance > 10)
			{
				FightsRanged = false;
				isInRange = false;
				//Combat();
				Idle();
				navAgent.SetDestination(this.transform.position);
			}
			*/
		}
	}
	void Patrol()
	{
		if (dead == false)
		{
			//GetComponent<Animation>().Play("run");
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
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		//GameObject g1 = GameObject.FindGameObjectWithTag("Player");
		//	pstat = g1.GetComponent<PlayerStats>();
		//p1 = g1.GetComponent<PlayerMover>();
	}

	void Update()
	{
		if(attackTimer < 550)
        {
			attackTimer++;
        }
		/*
		healthText.GetComponent<TextMeshProUGUI>().text = "Enemy Health: " + health;
		if (health <= 0)
		{

			//this.gameObject.setActive(false);
			//Destroy(enemy);
			Destroy(this);
			Debug.Log("ENEMY KILLED");
			death();

		}
		*/

		/*
		if(Input.GetButtonUp("Fire1"))
		{
			//Camera.main.SendMessage("stat_shot", 1);
			RangedAttack();
			
		}
		*/
		if (dead == false)
		{
			/*
			//healthRect.transform.localScale = new Vector3(health/100f, 0.1f, 0.1f);

			rAttackTimer = rAttackTimer + 1;
			mAttackTimer = mAttackTimer + 1;
			*/
			RunBehaviors();

		}
	}
}