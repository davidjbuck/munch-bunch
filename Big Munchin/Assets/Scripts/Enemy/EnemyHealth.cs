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

    //TAB ADDED: to differentiate different enemies (ie the birds)
    //for some reason dying got called twice so I'm also making a separate bool
    //so it doesn't spawn the body twice
    [SerializeField] private string enemyName;
    private bool alreadyDead = false;

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
        Debug.Log("ENEMY TOUCHED");
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

        //TAB ADDED: drops an object to pick up for the birds
        if (enemyName == "bird" && !alreadyDead)
        {
            //Debug.Log("y is: " + this.transform.position.y + 7);
            Vector3 positionWithOffset = new Vector3(this.transform.position.x, (21), this.transform.position.z);
            Instantiate(Resources.Load(enemyName), positionWithOffset, Quaternion.identity);
            alreadyDead = true;
        }


        Destroy(this.gameObject);
    }
    void Update()
    {
        
    }
}
