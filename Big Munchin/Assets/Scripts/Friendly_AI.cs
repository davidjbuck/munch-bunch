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

public class Friendly_AI : MonoBehaviour
{
	// Enum for different AI behaviors
	public Behaviors aiBehaviors = Behaviors.Idle;

	// Flags for interaction and player proximity
	public bool interactable;
	public bool nearPlayer;
	public bool interacting;

	// Rigidbody for physics interactions
	public Rigidbody rigidBod;

	// Speed settings for AI movement
	public float sprintSpeed;
	public float patrolSpeed;

	// NavMeshAgent for pathfinding
	UnityEngine.AI.NavMeshAgent navAgent;

	// Destination variables
	Vector3 Destination;
	Vector3 playerDestination;

	// Distance variables
	float Distance;
	float playerDistance;

	// Waypoints for patrolling
	public Transform[] Waypoints;
	int curWaypoint = 0;
	bool ReversePath = false;

	// Notification UI element
	public Image notification;

	// PlayerMover reference
	public PlayerMover p1;

	// Method for initiating NPC interaction
	void TalkToNPC()
	{
		Debug.Log("INTERACT WITH NPC");
		interacting = true;
	}

	// Method for patrolling behavior
	void Patrol()
	{
		// Get player position and calculate distance
		playerDestination = GameObject.FindGameObjectWithTag("Player").transform.position;
		playerDistance = Vector3.Distance(gameObject.transform.position, playerDestination);

		// Check if the player is near and interaction is possible
		if (playerDistance < 5f && interactable && !interacting)
		{
			nearPlayer = true;

			// Look at the player and stop moving
			Vector3 playerPosition = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
			this.transform.LookAt(playerPosition);
			navAgent.isStopped = true;
		}
		else
		{
			// Continue patrolling if not interacting with the player
			navAgent.isStopped = false;
			nearPlayer = false;
		}

		// Calculate distance to the current waypoint
		Distance = Vector3.Distance(gameObject.transform.position, Waypoints[curWaypoint].position);

		// Continue patrolling based on distance and reverse path if needed
		if (nearPlayer == false)
		{
			if (Distance > 5.00f)
			{
				Destination = Waypoints[curWaypoint].position;
				navAgent.SetDestination(Destination);
			}
			else
			{
				// Reverse path logic
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
				} else {
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
	}

	// Method called at the start of the script
	void Start()
	{
		// Get NavMeshAgent component and set initial notification visibility
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		notification.enabled = interactable;
	}

	// Method called when NPC interaction is finished
	void FinishedInteraction()
	{
		// Hide notification and disable further interaction
		notification.enabled = false;
		interactable = false;
		interacting = false;
	}

	// Method called in every frame update
	void Update()
	{
		// Execute patrolling behavior
		Patrol();

		// Check for interaction completion
		if (interacting && Input.GetKeyDown(KeyCode.R))
		{
			FinishedInteraction();
		}
		//Debug.Log(Distance);
	}
}



//FIRST SEMESTER CODE
/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;


public class Friendly_AI : MonoBehaviour
{

	public Behaviors aiBehaviors = Behaviors.Idle;
	public bool interactable;


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
	//public TMP_Text allyInteract;
	public Image notification;
	public PlayerMover p1;
	//public TMP_Text allyTextBox;
	//public GameObject allyText;
	//public GameObject interactPrompt;

	#region Behaviors
	void TalkToNPC()
    {
		//allyText.SetActive(true);
		//allyTextBox.enabled = true;
		Debug.Log("INTERACT WITH NPC");
		interacting = true;
		//interactPrompt.SetActive(false);
	}
	void Patrol()
	{
		playerDestination = GameObject.FindGameObjectWithTag("Player").transform.position;
		playerDistance = Vector3.Distance(gameObject.transform.position, playerDestination);
		//playerDistance = Vector3.Distance(gameObject.transform.position, this.transform.position);
		if(playerDistance < 5f && interactable && !interacting)
        {
			nearPlayer = true;
			Vector3 playerPosition = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, this.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
			this.transform.LookAt(playerPosition);
			//allyInteract.enabled = true;
			//interactPrompt.SetActive(true);
			navAgent.isStopped = true;
			
			//if (nearPlayer && Input.GetKeyDown(KeyCode.E))
			//{
			//	TalkToNPC();
			//}
			
		} else
        {
			//allyInteract.enabled = false;
			//interactPrompt.SetActive(false);
			navAgent.isStopped = false;

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

	#endregion



	void Start()
	{
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		if (interactable)
		{
			notification.enabled = true;
		}
		else
		{
			notification.enabled = false;
		}
	}

	void finishedInteraction()
    {
		//allyText.SetActive(false);

		notification.enabled = false;
		interactable = false;
		interacting = false;
	}

	void Update()
	{
		Patrol();
		if(interacting)
        {
			navAgent.isStopped = true;

			if (Input.GetKeyDown(KeyCode.R))
			{
				finishedInteraction();
			}
        }

	}
}
*/