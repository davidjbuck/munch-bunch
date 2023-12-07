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
    //ADD MORE SPAWN ARRAYS HERE IN THE FUTURE

    //public bool numEnemies;
    int spawnNum;
    public GameObject enemyPrefab;
    public float timer;
    public float spawnTimer;
    public float totalEnemies;
    public int spawnedEnemies;
    // Start is called before the first frame update
    void Start()
    {
        //TEMP CALL TO TEST SPAWNER
        //number of enemies - spawn timer - spawner number (for separate spawn locations)
        //spawnEnemies(3, 1, 2);
        //spawnEnemies(30, 5);
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
        spawnNum = Random.Range(0, enemySpawnpoints.Length);
        // Debug.Log("SPAWN ENEMY");
        Debug.Log(spawnNum);
        Instantiate(enemyPrefab, enemySpawnpoints[spawnNum].position, Quaternion.identity);
    }
}
 