using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitboxProperties : MonoBehaviour
{
    private MoveData move;//variable to hold MoveData
    public Transform anchorObject;//object to orient hitbox relative to
    public Vector3 offset;//offset from anchorObject
    public float timeEnabled;//how long to wait before creating hitbox after attack starts
    public float lifespan;//how long after hitbox is created to wait before destroying it
    public GameObject hitboxCollider;//a trigger prefab should be slotted in to act as the collider

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponentInParent<MoveData>();//grab MoveData component from parent MoveData
        if ((int)move.team == 0)
        {
            //anchorObject = GameObject.Find("player").transform;

            //anchorObject = this.transform;
            
            if (SceneManager.GetActiveScene().name == "KitchenScene")
            {
                anchorObject = this.transform;
            } else { 
                anchorObject = GameObject.Find("player").transform;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateHitbox()
    {
        //Debug.Log("Actually Creating Hitbox now");
        Vector3 pos = anchorObject.position + offset;//calculate position to place
        Vector3 dir = pos - anchorObject.transform.position;
        dir = anchorObject.rotation * dir;
        Vector3 rotatedPosition = dir + anchorObject.transform.position;
        GameObject hb = Instantiate(hitboxCollider, rotatedPosition, anchorObject.transform.rotation);//instantiate and store reference to hitbox
        hb.GetComponent<CollisionManager>().PassMoveData(move);//pass move data to hb
        hb.GetComponent<CollisionManager>().PassAttackProperties(move.GetAttackProperties().Clone());//pass attack properties to hb
        hb.GetComponent<CollisionManager>().SetAttackTeam((int)move.team);//pass attackTeam to hb
        hb.GetComponent<CollisionManager>().PassLifespan(lifespan);
        if (hb.GetComponent<CollisionManager>().GetAttackTeam() == 0)
        {
            ThirdPersonController tpc = GameObject.Find("Main Camera").GetComponent<ThirdPersonController>();
            if (tpc != null)
            {
                if (tpc.IsCrit())
                {
                    hb.GetComponent<CollisionManager>().UpdateAttackDamage(tpc.GetAttackIncrease());
                }
            }
            else
            {
                Debug.Log("Failed to find Main Camera when instantiating hitbox...");
            }
        }
    }

    public float GetTimeEnabled()
    {
        //Debug.Log("Passing timeEnabled: " + timeEnabled);
        return timeEnabled;
    }

    public float GetTimeDisabled()
    {
        //Debug.Log("Passing timeDisabled: " + lifespan);
        return lifespan;
    }
}
