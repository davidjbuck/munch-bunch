using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float enemyMaxHP;
    public float enemyHP;
    [SerializeField] private Slider hSlider;

    // Start is called before the first frame update
    void Start()
    {
        enemyHP = enemyMaxHP;
    }
    void removeHealth(float damage)
    {
        enemyHP = enemyHP - damage;
        updateEnemyHealthBar(enemyHP, enemyMaxHP);
        if(enemyHP < 0)
        {
            Debug.Log("ENEMY DEAD");
            Destroy(this.gameObject);
        }
    }
    public void updateEnemyHealthBar(float hp, float maxHealth)
    {
        hSlider.value = hp / maxHealth;
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Hitbox") 
        {
            //attackProps = CollisionManager.GetComponent<CollisionManager>();
            removeHealth(1);
        }
    }

    void Update()
    {
        
    }
}
