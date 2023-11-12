using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveData : MonoBehaviour
{
    public AttackTeam team;//0 for player, 1 for enemy
    public GameObject[] hitboxPropertyCollection;//array of GameObjects with HitboxProperties components
    private HitboxProperties[] hitboxData;//array to hold HitboxProperties for each hitbox on the move
    public GameObject attackPropertiesHolder;//GameObject with AttackProperties component
    private AttackProperties attackProps;//variable to hold AttackProperties for the move
    private float maxLifespan = 0.0f;
    public float staminaCost;
    public enum AttackTeam
    {
        Player=0,
        Enemy =1
    }
    // Start is called before the first frame update
    void Start()
    {
        hitboxData = new HitboxProperties[hitboxPropertyCollection.Length];//initialize array to match size
        for(int i = 0;i< hitboxPropertyCollection.Length;i++) { 
            hitboxData[i] = hitboxPropertyCollection[i].GetComponent<HitboxProperties>();
            if (hitboxPropertyCollection[i].GetComponent<HitboxProperties>().GetTimeDisabled() + hitboxPropertyCollection[i].GetComponent<HitboxProperties>().GetTimeEnabled() > maxLifespan)
            {
                maxLifespan = hitboxPropertyCollection[i].GetComponent<HitboxProperties>().GetTimeDisabled() + hitboxPropertyCollection[i].GetComponent<HitboxProperties>().GetTimeEnabled();
                //Debug.Log(hitboxData[i].GetTimeDisabled());
            }
        }//and then copy over each HitboxProperties component to the hitboxData array
        attackProps = attackPropertiesHolder.GetComponent<AttackProperties>();//copy over AttackProperties component to attackProps
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMoveAnimation()
    {
        //connect to animator later in integration
    }

    public AttackProperties GetAttackProperties()
    {
        return attackProps;
    }

    public HitboxProperties[] GetHitboxProperties()
    {
        //Debug.Log("Returning HitboxProperties with length: " + hitboxData.Length);
        return hitboxData;
    }

    public float GetMaxLifespan()
    {
        return maxLifespan;
    }

    public float GetStaminaCost()
    {
        return staminaCost;
    }
}
