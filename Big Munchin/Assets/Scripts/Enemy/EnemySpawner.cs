using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//THIS SCRIPT IS CUSTOMIZED FOR THE FIRST ENCOUNTER, BUT PARTS OF THE ENEMY SPAWNER CAN BE USED IN OTHER PLACES
public class EnemySpawner : MonoBehaviour
{
    public bool spawnActive;
    //this one uses other arrays to set the spawn point later on
    Transform[] enemySpawnpoints;
    //custom waypoint transforms
    //THIS JUST USES EMPTY TRANSFORMS WITH Y OF 0
    public Transform[] wave1SpawnPoints;
    public Transform[] wave2SpawnPoints;
    public Transform tonySpawn;
    //ADD MORE SPAWN ARRAYS HERE IN THE FUTURE

    //public bool numEnemies;
    int spawnNum;
    public GameObject enemyPrefab;
    public GameObject tonyPrefab;
    public GameObject invWalls;
    public GameObject wave2Trigger;
    float timer;
    float spawnTimer;
    float totalEnemies;
    int spawnedEnemies;
    bool wave1 = true;
    bool wave2 = false;
    int spawner;
    int counter;
    bool wave1Completed = false;
    public static int enemyDeathCounter;
    int wave1Enemies;
    bool continuousSpawns;
    public GameObject run1;
    public GameObject run2;
    public GameObject run3;
    // Start is called before the first frame update
    void Start()
    {
       // runEnemies = false;
        enemyDeathCounter = 0;
        //no continuous spawns
        continuousSpawnsOff();

        //THESE ARE TEMP CALLS TO TEST SPAWNER FOR EACH SCENARIO (PROBABLY USE ONE SIMILAR FOR FINAL DEMO, BUT CAN HAVE FIRST TWO NUMBERS TWEAKED)
        //number of enemies/spawn timer/spawner number (for separate spawn locations)

        //INITIAL ENCOUNTER IS 1
        //spawnEnemies(3, 0, 1);

        //SECOND ENCOUNTER IS 2 (WHERE THEY COME FROM TONYS SHOP)
        //spawnEnemies(4, 1, 2);

        //TONY SPAWN IS 3 (MUST BE 1,0,3)
        //spawnEnemies(1, 0, 3);
        wave1Enemies = 3; 
        /*
        run1.RunFleeNode();
        run2.RunFleeNode();
        run3.RunFleeNode();
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnActive)
        {

            timer += Time.deltaTime;
          //  Debug.Log(timer);

            if (spawnedEnemies < totalEnemies)
            {
                if (timer > spawnTimer)
                {
                    //Debug.Log(spawnedEnemies + " " + totalEnemies);

                    spawn();
                    counter++;
                    spawnedEnemies++;
                    timer = 0;
                    if (spawnedEnemies == totalEnemies)
                    {

                        counter = 0;
                        spawnActive = false;

                        wave2 = false;
                    }
                }
            }

        }
        else if (continuousSpawns)
        {
           // Debug.Log("CONTINE SPAWNING");
            spawnEnemies(3, 4, 2);
        }
        //Debug.Log(enemyDeathCounter + "EDC");
        if (enemyDeathCounter == 3 && !wave1Completed)
        {
            //Debug.Log("WALLS DOWN");
            invWalls.SetActive(false);
            wave2Trigger.SetActive(true);
            wave1Completed = true;
            /*
            run1.GetComponent<SpawnedEnemyAI>().wave1RunningEnemies = true;
            run2.GetComponent<SpawnedEnemyAI>().wave1RunningEnemies = true;
            run3.GetComponent<SpawnedEnemyAI>().wave1RunningEnemies = true;
            */
            run1.GetComponent<SpawnedEnemyAI>().FleeToRestaurant();
            run2.GetComponent<SpawnedEnemyAI>().FleeToRestaurant();
            run3.GetComponent<SpawnedEnemyAI>().FleeToRestaurant();

            //  runEnemies = true;
            Debug.Log("ENEMIES RUN");
        }
        
        /* TEST TO HAVE CONTINUOUS SPAWNS (but no delay between them)

        */
    }
    //SETS SPAWNS WITH NUM ENEMIES, SPAWN TIME, AND SPAWNPOINT
    //CALL IT AFTER WAVE FINISHES
    public void spawnEnemies(int enemies, float spawnTime, int spawnN)
    {
        spawnTimer = spawnTime;
        timer += Time.deltaTime;
        spawnActive = true;
      //  Debug.Log(timer);
        totalEnemies = enemies;
        spawnedEnemies = 0;
        spawner = spawnN;
        //SETS CURRENT WAYPOINTS TO DESIRED SPAWNS
        if(spawnN == 1)
        {
            wave1 = true;
            enemySpawnpoints = wave1SpawnPoints;
            wave1Enemies = enemies;
        }
        if(spawnN == 2)
        {
            wave2 = true;
            enemySpawnpoints = wave2SpawnPoints;
        }

        //ADD MORE WAVE CHECKS HERE

    } 
    public void continuousSpawnsOn()
    {
        continuousSpawns = true;
    }
    public void continuousSpawnsOff()
    {
        continuousSpawns = false;
    }
    public void spawn()
    {
        // Debug.Log("SPAWN ENEMY");

        //FOR FIRST ENCOUNTER, ENEMIES ARE SPAWNED IN SET LOCATIONS, SO THERES NO CHANCE OF ALL OF THEM SPAWNING AT THE SAME TIME
        //ALLOWS FOR INSTANT SPAWNING FOR ENEMIES WAITING AS SOON AS PLAYER GETS OUT
        if (spawner == 1)
        {
            Instantiate(enemyPrefab, enemySpawnpoints[counter].position, Quaternion.identity);
        }

        //RANDOM SPAWN LOCATIONS (CURRENTLY SIDE DOOR AND BACK OF RESTAURANT COULD ADD TONY'S SINCE THAT IS IN FRONT OF RESTAURANT
        if (spawner == 2)
        {
            Debug.Log(spawnNum);

            spawnNum = Random.Range(0, enemySpawnpoints.Length);

            Instantiate(enemyPrefab, enemySpawnpoints[spawnNum].position, Quaternion.identity);
        }

        //SPAWNS TONY RIGHT IN FRONT OF RESTAURANT, THEN HE RUNS OFF TO A WAYPOINT IN THE WOODS
        if(spawner == 3)
        {
            Instantiate(tonyPrefab, tonySpawn.position, Quaternion.identity);
        }
    }
}
 