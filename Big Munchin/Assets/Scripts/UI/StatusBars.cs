using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusBars : MonoBehaviour
{
    public Slider slider;
    public Slider staminaSlider;
    public Slider hungerSlider;
    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetMaxHealth (int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetStamina (float stamina)
    {
        staminaSlider.value = stamina;
    }

    public void SetMaxStamina(float stamina)
    {
        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;
    } 
    
    public void SetHunger(float hunger)
    {
        hungerSlider.value = hunger;
    }

    public void SetMaxHunger(float hunger)
    {
        hungerSlider.maxValue = hunger;
        hungerSlider.value = hunger;
    }
}
