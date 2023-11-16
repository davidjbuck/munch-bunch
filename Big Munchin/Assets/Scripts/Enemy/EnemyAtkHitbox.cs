using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//NOT USED ANYMORE
public class EnemyAtkHitBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("Hit Player");
        }
    }
    /*
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Hit Player");
        }
    }
    */
}
