
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

//public enum Behaviors { Idle, Guard, Combat, Flee, FleeToRestaurant };

public class SpawnedEnemyAI : MonoBehaviour
{
    public Behaviors aiBehaviors = Behaviors.Idle;
    public bool dead = false;
    public bool isSuspicious = false;
    public bool isInRange = false;
    public bool FightsRanged = false;
    public float attackTimer;
    bool aggressive = true;
    bool permAggressive = true;
    public Rigidbody rigidBod;
    public float sprintSpeed;
    public float patrolSpeed;
    UnityEngine.AI.NavMeshAgent navAgent;
    Vector3 Destination;
    Vector3 PlayerDestination;
    float Distance;
    public PlayerMover p1;
    public Image lightAttackWarning;
    public Image heavyAttackWarning;
    public float lightAttackCooldown;
    public float heavyAttackCooldown;
    public GameObject enemyAttack;
    public Transform attackSpawn;
    public bool wave1RunningEnemies;
    private int randNum;
    private bool newRand;
    private float attackCooldown = 0; // Added variable
    public MovesetHolder[] enemyMovesets;
    MovesetHolder enemyActiveMoveset;
    GameObject player;
    private bool movingToRestaurant;
    private string stateCheck;
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
            case Behaviors.FleeToRestaurant:
                RunFleeToRestaurantNode();
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
    }

    void RunFleeNode()
    {
        Flee();
    }

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
            PlayerDestination = player.transform.position;
            Distance = Vector3.Distance(gameObject.transform.position, PlayerDestination);
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
                GetComponent<UnityEngine.AI.NavMeshAgent>().speed = sprintSpeed;

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
       // Destination = player.transform.position;


        //engaging();
        if (dead == false)
        {
            if (Distance < 2)
            {
                // Destination = this.transform.position;
                //Destination = this.transform.position;

                Vector3 playerPosition = new Vector3(player.transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
                this.transform.LookAt(playerPosition);

                if (attackTimer >= (attackCooldown - 1) && randNum == 0)
                {
                   // Destination = this.transform.position;
                    //attacking();

                    lightAttackWarning.enabled = true;
                   // Destination = this.transform.position;
                }
                else if (attackTimer >= (attackCooldown - 1) && randNum == 1)
                {
                    //Destination = this.transform.position;
                    //attacking();

                    heavyAttackWarning.enabled = true;
                  //  Destination = this.transform.position;

                }
                else
                {
                    lightAttackWarning.enabled = false;
                    heavyAttackWarning.enabled = false;
                    //Destination = this.transform.position;
                    //attacking();
                    //Destination = transform.position - transform.forward * 4f; // Adjust the distance as needed

                }

                if (attackTimer <= (attackCooldown))
                {
                    attackTimer += Time.deltaTime;
                    //Destination = this.transform.position;

                    /*
                    if (lightAttackWarning.enabled == false && heavyAttackWarning.enabled == false)
                    {
                        Destination = transform.position - transform.forward * 2f; // Adjust the distance as needed
                    } else
                    {
                        Destination = this.transform.position;
                    }
                    */

                    // Debug.Log(Destination);
                    //navAgent.SetDestination(Destination);
                    //Debug.Log("MOVE BACK");
                }

                if (attackTimer > attackCooldown)
                {
                    //Destination = this.transform.position;
                    //attacking();
                    if (randNum == 0)
                    {
                        enemyActiveMoveset.LightAttackCombo();
                    }
                    if (randNum == 1)
                    {
                        enemyActiveMoveset.HeavyAttackCombo();
                        attackCooldown = attackCooldown - 1;
                    }

                    newRand = false;
                    attackTimer = 0;
                //    movingBack();

                    // Destination = this.transform.position;


                    //  lightAttackWarning.enabled = false;
                    //   heavyAttackWarning.enabled = false;
                }
                Destination = this.transform.position;
                navAgent.SetDestination(Destination);

            }
            if (Distance > 5)
            {
                // attackTimer = 200;
                if (attackTimer > attackCooldown)
                {

                    //engaging();
                    //Destination = player.transform.position;
                    navAgent.SetDestination(PlayerDestination);

                }
                else
                {
                    //waiting();
                  
                   // Destination = this.transform.position;
                }
                //Destination = player.transform.position;
                //                navAgent.SetDestination(player.transform.position);
            }
            else
            {
                //attackTimer = 200;
                SearchForTarget();
            }/*
            if (attacking)
            {
                Destination = this.transform.position;
                stateCheck = "Attacking";
            }
            if (waiting)
            {
                Destination = this.transform.position;
                stateCheck = "Waiting";

            }
            if (movingBack)
            {
                Destination = transform.position - transform.forward * 2f; // Adjust the distance as needed
                stateCheck = "Moving Back";

            }
            if (engaging)
            {
                Destination = player.transform.position;
                stateCheck = "Engaging";

            }
            */
           // navAgent.SetDestination(Destination);

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
            movingToRestaurant = true;
        }
    }

    void GuardSearchForTarget()
    {
        if (dead == false)
        {
            if (Distance < 40f)
            {
                if (Distance < 10f)
                {
                    Destination = player.transform.position;
                    navAgent.SetDestination(Destination);
                    if (Distance < 6f)
                    {
                        if (attackTimer < 4)
                        {
                            if (Distance < 4)
                            {
                                Destination = transform.position - transform.forward * 2f; // Adjust the distance as needed
                            }
                            else
                            {
                                Destination = this.transform.position;
                            }
                            navAgent.SetDestination(Destination);

                            lightAttackWarning.enabled = false;
                            heavyAttackWarning.enabled = false;
                            Vector3 playerPosition = new Vector3(player.transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
                            this.transform.LookAt(playerPosition);
                        }
                        else
                        {
                            Debug.Log("ATTACK");
                            // Destination = player.transform.position;
                            Combat();
                            if (Distance < 2)
                            {
                                Destination = this.transform.position;
                                navAgent.SetDestination(Destination);
                            }
                            else
                            {
                                Destination = player.transform.position;
                                navAgent.SetDestination(Destination);
                            }
                        }
                    }
                    else
                    {
                        lightAttackWarning.enabled = false;
                        heavyAttackWarning.enabled = false;
                    }
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
        GuardSearchForTarget();
    }

    void OnCollisionEnter(Collision col)
    {
    }

    void death()
    {
        // ... (existing code for death)
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
    }

    void Update()
    {
        Debug.Log(aiBehaviors);
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

        if (attackTimer < (attackCooldown - 1))
        {
            attackTimer += Time.deltaTime;
        }

        if (dead == false)
        {
            if (movingToRestaurant)
            {
                Distance = Vector3.Distance(transform.position, Destination);
                if (Distance < 1.00f)
                {
                    Destroy(this.gameObject);
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
