using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void OnCollisionEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("Hit Player");
        }
    } 
}
