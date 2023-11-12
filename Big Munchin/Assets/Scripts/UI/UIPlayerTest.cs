using UnityEngine;

public class UIPlayerTest : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public bool isHealthRegenerating = false;

    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegen = .03f;

    public float maxHunger = 100f;
    public float currentHunger;

    public StatusBars healthBar; //Slider value 
    public StatusBars staminaBar;
    public StatusBars hungerBar;

        void Start()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentHunger = maxHunger;
            healthBar.SetMaxHealth(maxHealth);
            staminaBar.SetMaxStamina(maxStamina);
            hungerBar.SetMaxHunger(maxHunger);
        }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0,0,3) * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerDamage(15);
        }
        if(currentHealth < maxHealth && isHealthRegenerating)
        {
            RegenerateHealth(1/33);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            DrainStamina(25f);
        }
        if (currentStamina < maxStamina)
        {
            RegenerateStamina(staminaRegen);
        }
        
        if(Input.GetKeyDown(KeyCode.D))
        {
            DrainHunger(20f);
        }

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
