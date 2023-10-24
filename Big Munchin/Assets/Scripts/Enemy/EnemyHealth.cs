using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float enemyMaxHP;
    public float enemyHP;
    public bool enemyAlive;
    [SerializeField] private Slider hSlider;

    // Start is called before the first frame update
    void Start()
    {
        enemyHP = enemyMaxHP;
        enemyAlive = true;
    }
    void removeHealth(float damage)
    {
        enemyHP = enemyHP - damage;
        updateEnemyHealthBar(enemyHP, enemyMaxHP);
        if(enemyHP < 0)
        {
            enemyAlive = false;
            EnemyDead();
        }
    }
    public void updateEnemyHealthBar(float hp, float maxHealth)
    {
        hSlider.value = hp / maxHealth;
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Hitbox" && col.GetComponent<CollisionManager>().GetAttackTeam() == 0) 
        {
            //attackProps = CollisionManager.GetComponent<CollisionManager>();
            float damage = col.GetComponent<CollisionManager>().GetAttackDamage();
            Debug.Log(damage);
            removeHealth(damage);
        }
    }
    public void EnemyDead()
    {
        Destroy(this.gameObject);

    }
    void Update()
    {
        
    }
}
