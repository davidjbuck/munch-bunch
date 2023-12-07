using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SPAWN TRIGGERS USE AN EMPTY GAMEOBJECT AS A PARENT, THIS SCRIPT GOES ONTO THAT
//CHILDREN ARE THE ACTUALL BOX COLLIDERS, SO ONCE ONE IS HIT ALL ARE DESTROYED
public class SpawnTrigger : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider trg)
    {
        //Debug.Log("Spawn Enemies: " + trg.tag);
        
        if (trg.tag == "Player")
        {
            enemySpawner.spawnEnemies(3, 1, 2);
            Destroy(this);
        }
    }
}
