using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meatball : MonoBehaviour
{
    public MovesetHolder[] enemyMovesets;
    MovesetHolder enemyActiveMoveset;

    // Start is called before the first frame update
    void Start()
    {
        enemyActiveMoveset = enemyMovesets[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("HIT PLAYER");
            //spawn hitbox
            enemyActiveMoveset.LightAttackCombo();
            //Destroy(this);

        }
        //Debug.Log("MEATBALL HIT");
    }
}
