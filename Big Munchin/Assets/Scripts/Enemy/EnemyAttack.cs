using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 1000 * Time.deltaTime, 0); //rotates 25 degrees per second around y axis
        if(this.transform.rotation.z > 60)
        {
            Destroy(this.gameObject);
        }
    }
}
