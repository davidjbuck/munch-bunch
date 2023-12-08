using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TonyRunAI : MonoBehaviour
{
    Transform waypoint;
    public bool running;
    UnityEngine.AI.NavMeshAgent navAgent;
    Vector3 Destination;
    //public GameObject Player;
    float distance;
    float speedDecrease;
    // Start is called before the first frame update
    void Start()
    {
        waypoint = GameObject.FindGameObjectWithTag("tonyWaypoint").transform;
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        speedDecrease = 0.01f;
        //startRun();
    }
    public void startRun()
    {
        StartCoroutine("SlowSpeedPerSecond", 1f);
        Destination = waypoint.position;
        navAgent.SetDestination(Destination);
        running = true;
    }

    IEnumerator SlowSpeedPerSecond(float waitTime)
    {
        while(navAgent.speed > 0)
        {
            yield return new WaitForSeconds(waitTime);
            speedDecrease = speedDecrease * 1.15f;
            navAgent.speed = navAgent.speed - speedDecrease;
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, waypoint.position);
        if(distance <= 10)
        {
            //END GAME HERE
            SceneManager.LoadScene("End Screen");
        }
        //Debug.Log(distance);

        /*
        distance = Vector3.Distance(Player.transform.position, this.transform.position);
        if(distance < 10f)
        {
            //startRun();
        }
        if (running)
        {
            Debug.Log(distance);
        }
        */
    }
}
