using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//SPAWN TRIGGERS USE AN EMPTY GAMEOBJECT AS A PARENT, THIS SCRIPT GOES ONTO THAT
//CHILDREN ARE THE ACTUALL BOX COLLIDERS, SO ONCE ONE IS HIT ALL ARE DESTROYED
public class SpawnTrigger : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public GameObject spawnTrigger;
    public TonyRunAI tony;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.tag);
        
        if (col.tag == "Player")
        {
            //put detective code here





            //old version for tony running
            /*
            enemySpawner.spawnEnemies(4, 1, 2);
            enemySpawner.continuousSpawnsOn();
            tony.startRun();
            Destroy(spawnTrigger);
            */
        }
    }
}
 