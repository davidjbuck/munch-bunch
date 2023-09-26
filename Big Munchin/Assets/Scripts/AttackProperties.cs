using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackProperties : MonoBehaviour
{
    public AttackType at;//enum to hold the type of attack
    public int damage;//how much damage it deals
    public float stunDuration;//how long the attack stuns if it hits
    public float knockbackForce;//how much knockback this deals. should be set to 0 and ignored unless attack is a heavy finisher

    public enum AttackType
    {
        Light = 0,
        Heavy = 1,
        Special = 2,
        LightFinisher = 3,
        HeavyFinsher =4
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
