using UnityEngine;

public class UIPlayerTest : MonoBehaviour
{
    public ThirdPersonController player;
    public int maxHP;
    public int currentHealth;
    public bool isHealthRegenerating = false;

    public float maxStam;
    public float currentStamina;
    public float staminaRegen = .05f;

    public float maxHunger;
    public float currentHunger;

    public StatusBars healthBar; //Slider value 
    public StatusBars staminaBar;
    public StatusBars hungerBar;
    public GameObject staminaBarLength;

    private Vector3 barLengthIncrease = new Vector3(1.2f, 0.7f, 1.0f);
        void Start()
        {
            maxHP = player.maxHealth;
            maxStam = player.maxStamina;

            currentHealth = maxHP;
            currentStamina = maxStam;
            currentHunger = maxHunger;

            healthBar.SetMaxHealth(maxHP);
            staminaBar.SetMaxStamina(maxStam);
            hungerBar.SetMaxHunger(maxHunger);
        }

    // Update is called once per frame
    void Update()
    {
        currentHealth = player.health;
        currentStamina = player.stamina;
        maxStam = player.maxStamina;
        maxHP = player.maxHealth;

        //print(player.maxStamina);

        if (player.stamina > 100f)
        {
            staminaBarLength.transform.localScale = barLengthIncrease;
        }
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerDamage(15);
        }*/
        if (currentHealth < maxHP && isHealthRegenerating)
        {
            RegenerateHealth(1/33);
        }

        /*if(Input.GetKeyDown(KeyCode.S))
        {
            DrainStamina(25f);
        }*/
        if (currentStamina < maxStam)
        {
            RegenerateStamina(staminaRegen);
        }

        if (currentStamina <= 0f)
        {
            currentStamina = 0f;
        }
        if(player.health < maxHP)
        {
            currentHealth = player.health;
            healthBar.SetHealth(currentHealth);
        }
        /*        if(Input.GetKeyDown(KeyCode.D))
                {
                    DrainHunger(20f);
                }*/

        DegenerateHunger(.0005f);
    }

    void PlayerDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    void RegenerateHealth(int recover)
    {
        currentHealth += recover;
        if (currentHealth > 100)
        {
            currentHealth = 100;
        }
        healthBar.SetHealth(currentHealth);
    }
    void DrainStamina(float drain)
    {
        currentStamina -= drain;

        staminaBar.SetStamina(currentStamina);
    }

    void RegenerateStamina(float recover)
    {
        currentStamina += recover;
        if (currentStamina > 100f)
        {
            currentStamina = 100f;
        }
        staminaBar.SetStamina(currentStamina);
    }

    void DrainHunger(float drain)
    {
        currentHunger -= drain;

        hungerBar.SetHunger(currentHunger);
    }

    void DegenerateHunger(float degen)
    {
        currentHunger -= degen;

        hungerBar.SetHunger(currentHunger);
    }
}
