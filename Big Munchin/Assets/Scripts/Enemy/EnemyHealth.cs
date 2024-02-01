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
    private bool deathCounted = false;
    //public GameObject itemDropped;
    //public int numDropped;
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
            //Debug.Log(damage); 
            removeHealth(damage);
        }
    }
    public void EnemyDead()
    {
        if (!deathCounted)
        {
            EnemySpawner.enemyDeathCounter++;
            //Debug.Log(EnemySpawner.enemyDeathCounter + "EDC");

            deathCounted = true;
        }
        /*
        if(numDropped!= 0)
        {
            Debug.Log("DROP ITEM");
            for(int i = 0; i < numDropped; i++)
            {
                Instantiate(itemDropped, this.transform.position, Quaternion.identity);

            }
        }
        */
        Destroy(this.gameObject);

    }
    void Update()
    {
        
    }
}
