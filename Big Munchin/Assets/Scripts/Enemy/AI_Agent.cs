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

public enum Behaviors { Idle, Guard, Combat, Flee, FleeToRestaurant };

public class AI_Agent : MonoBehaviour
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
	public int rAttackTimer;
	public int mAttackTimer;
	//public GameObject Projectile;
	public Rigidbody rigidBod;

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
		//	Destination = GameObject.FindGameObjectWithTag("Player").transform.position;
			//Distance = Vector3.Distance(gameObject.transform.position, Destination);
			if (Distance < 10f)
			{
				isSuspicious = true;
			}
			if (isSuspicious)
			{
				GuardSearchForTarget();
			}
			else
			{
				Patrol();
			}
		}
	}

	void Combat()
	{
		if (dead == false)
		{
			if (isInRange)
			{
				if (FightsRanged)
				{
					if (rAttackTimer > 200)
					{
						RangedAttack();
						rAttackTimer = 0;
					}
				}
				else
				{
					if (mAttackTimer > 800)
					{
						MeleeAttack();
						mAttackTimer = 0;
					}
				}
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
			//GetComponent<Animation>().Play("run");
			for (int fleePoint = 0; fleePoint < Waypoints.Length; fleePoint++)
			{
				Distance = Vector3.Distance(gameObject.transform.position, Waypoints[fleePoint].position);
				if (Distance > 10.00f)
				{
					Destination = Waypoints[curWaypoint].position;
					navAgent.SetDestination(Destination);
					break;
				}
				else if (Distance < 2.00f)
				{
					ChangeBehavior(Behaviors.Idle);
				}
			}
		}
	}
	void GuardSearchForTarget()
	{
		if (dead == false)
		{
			if (Distance < 10f)
			{
				if (Distance < 2f)
				{
					if (mAttackTimer > 200)
					{
					//	GetComponent<Animation>().Play("attack");
						//pstat.removeHealth(15);
						mAttackTimer = 0;
					}
					navAgent.SetDestination(this.transform.position);
					navAgent.isStopped = true;

					//navAgent.Stop();

				}
				else
				{
					navAgent.isStopped = false;
					//navAgent.Resume();
					navAgent.SetDestination(Destination);

					//GetComponent<Animation>().Play("run");
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

				if (Distance < 5f)
				{
					if (rAttackTimer > 200)
					{
						//	GetComponent<Animation>().Play("attack");
						Debug.Log("Enemy Fire");

						/*
						GameObject newProjectile;
						newProjectile = Instantiate(Projectile, shotSpawn.position, shotSpawn.rotation) as GameObject;
						Destroy(newProjectile, 3);

						GameObject newProjectile1;
						newProjectile1 = Instantiate(Projectile, shotSpawn1.position, shotSpawn1.rotation) as GameObject;
						Destroy(newProjectile1, 3);
						rAttackTimer = 0;
						*/
					}
					//navAgent.LookAt(Destination);
					//navAgent.isStopped = true;

					//navAgent.Stop();

				}
				else
				{
					navAgent.isStopped = false;

					//navAgent.Resume();
					navAgent.SetDestination(Destination);

					//GetComponent<Animation>().Play("run");
				}
			//	GetComponent<Animation>().Play("run");

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
	void RangedAttack()
	{
		if (dead == false)
		{

			/*
			//GetComponent<Animation>().Play("attack");
			GameObject newProjectile;
			newProjectile = Instantiate(Projectile, transform.position, this.transform.rotation) as GameObject;
			Destroy(newProjectile, 5);
			*/
		}
	}
	void MeleeAttack()
	{
		if (dead == false)
		{
			//GetComponent<Animation>().Play("attack");

		//	pstat.removeHealth(30);
		}
	}
	#endregion


	/*
	void removeHealth(int rHealth)
	{
		health = health - rHealth;
	}
	*/
	void OnCollisionEnter(Collision col)
	{


		/*

				if (col.gameObject.tag == "bullet")
				{
					Destroy(col.gameObject);
					health = health - 20;
					Debug.Log("ENEMY HIT: " + health);
					rigidBod.constraints = RigidbodyConstraints.None;

					//removeHealth(20);
					//  Camera.main.SendMessage("stat_hit", 1);
					//  Camera.main.SendMessage("stat_health", -10);
				}
				if (col.gameObject.tag == "Bullet2")
				{
					Destroy(col.gameObject);
					health = health - 20;
					Debug.Log("ENEMY HIT: " + health);
					rigidBod.constraints = RigidbodyConstraints.None;

					//removeHealth(20);
					//  Camera.main.SendMessage("stat_hit", 1);
					//  Camera.main.SendMessage("stat_health", -10);
				}
				if (col.gameObject.tag == "Spear")
				{
					Destroy(col.gameObject);
					health = health - 100;
					Debug.Log("ENEMY HIT: " + health);
					//removeHealth(20);
					//  Camera.main.SendMessage("stat_hit", 1);
					//  Camera.main.SendMessage("stat_health", -10);
				}
			}
			*/



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
			Patrol();
			
		}
	}
}