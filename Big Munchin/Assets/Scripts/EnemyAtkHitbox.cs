using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtkHitbox : MonoBehaviour
{
    public void OnCollisionEnter(Collision col)
    {
        Debug.Log("HIT something");
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("HIT PLAYER");
        }
    }
}
