using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool spawnActive;
    public Transform[] enemySpawnpoints;
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
        spawnEnemies(3, 1);
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
                    Debug.Log(spawnedEnemies + " " + totalEnemies);

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


    }
    public void spawnEnemies(int enemies, float spawnTime)
    {
        spawnTimer = spawnTime;
        timer += Time.deltaTime;
        spawnActive = true;
        Debug.Log(timer);
        totalEnemies = enemies;
        spawnedEnemies = 0;

    }
    public void spawn()
    {
        spawnNum = Random.Range(0, enemySpawnpoints.Length+1);
        Debug.Log("SPAWN ENEMY");
        Instantiate(enemyPrefab, enemySpawnpoints[spawnNum].position, Quaternion.identity);
    }
}
