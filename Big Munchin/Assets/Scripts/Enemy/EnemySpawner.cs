using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float timer;
    float spawnTimer;
    float totalEnemies;
    int spawnedEnemies;
    int spawner;
    int counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;


        //THESE ARE TEMP CALLS TO TEST SPAWNER FOR EACH SCENARIO (PROBABLY USE ONE SIMILAR FOR FINAL DEMO, BUT CAN HAVE FIRST TWO NUMBERS TWEAKED)
        //number of enemies/spawn timer/spawner number (for separate spawn locations)

        //INITIAL ENCOUNTER IS 1
        //spawnEnemies(3, 0, 1);

        //SECOND ENCOUNTER IS 2 (WHERE THEY COME FROM TONYS SHOP)
        //spawnEnemies(4, 1, 2);

        //TONY SPAWN IS 3 (MUST BE 1,0,3)
        //spawnEnemies(1, 0, 3);
        wave1Enemies = 3;
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
                        spawnActive = false;

                    }
                }
            }
        } 
        /* TEST TO HAVE CONTINUOUS SPAWNS (but no delay between them)
        else
        {
            //spawnEnemies(Random.Range(0, 5), Random.Range(1, 3), Random.Range(0, 2));
        }
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
            enemySpawnpoints = wave1SpawnPoints;
        }
        if(spawnN == 2)
        {
            enemySpawnpoints = wave2SpawnPoints;
        }

        //ADD MORE WAVE CHECKS HERE

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
 