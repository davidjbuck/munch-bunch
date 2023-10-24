using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private MoveData md;
    private AttackProperties ap;
    private int attackTeam = -1;//0 for player, 1 for enemy. set to -1 as sentinel value
    private float timeSpawned;
    private float lifespan;
    // Start is called before the first frame update
    void Start()
    {
        timeSpawned = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > timeSpawned + lifespan)
        {
            Destroy(gameObject);
        }
    }

    public void PassMoveData(MoveData move)
    {
        md = move;
    }

    public void PassAttackProperties(AttackProperties attackProps)
    {
        ap = attackProps;
    }

    public void UpdateAttackDamage(float attackDamageChange)
    {
        ap.UpdateDamage(attackDamageChange);
    }

    public void PassLifespan(float ls)
    {
        lifespan = ls;
    }

    public void SetAttackTeam(int team)
    {
        attackTeam = team;
    }

    public int GetAttackTeam()
    {
        return attackTeam;
    }

    public int GetAttackDamage()
    {
        return ap.damage;
    }

    public float GetAttackStun()
    {
        return ap.stunDuration;
    }

    public float GetKnockbackForce()
    {
        return ap.knockbackForce;
    }

    public AttackProperties GetAttackProperties()
    {
        return ap;
    }
}
