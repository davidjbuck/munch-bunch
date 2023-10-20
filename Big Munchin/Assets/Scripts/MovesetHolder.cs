using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesetHolder : MonoBehaviour
{
    public GameObject[] lightAttacks;//public array of GameObjects. Each should have a MoveData component for light attacks
    private MoveData[] lights;
    public GameObject[] heavyAttacks;//public array of GameObjects. Each should have a MoveData component for heavy attacks
    private MoveData[] heavies;
    public GameObject[] specialAttacks;//public array of GameObjects. Each should have a MoveData component for special attacks
    private MoveData[] specials;

    private bool attacking;
    private float attackStartTime;
    private int currentMove = -1;
    private int attackMode = -1;
    List<float> moveStartTimes;
    private float lastHitboxOutTime;
    private float lastAttackRequestTime;
    public float attackBufferWindow;
    public bool attachedToPlayer;//should be set to true for player, false for AI
    private ThirdPersonController tpc;
    private float[] lightsStaminaCosts;
    private float[] heaviesStaminaCosts;
    private bool staminaCostPaid = false;

    // Start is called before the first frame update
    void Start()
    {
        moveStartTimes = new List<float>();
        if(attachedToPlayer)
        {
            tpc = GameObject.Find("Main Camera").GetComponent<ThirdPersonController>();
        }
        lastAttackRequestTime = Time.time;
        lights = new MoveData[lightAttacks.Length];
        lightsStaminaCosts = new float[lightAttacks.Length];
        for(int i = 0; i < lightAttacks.Length; i++)
        {
            lights[i] = lightAttacks[i].GetComponent<MoveData>();
            lightsStaminaCosts[i] = lights[i].GetStaminaCost();
        }//copy light attack MoveData over to lights
        //Debug.Log("lights.Length: " + lights.Length);
        heavies = new MoveData[heavyAttacks.Length];
        heaviesStaminaCosts = new float[heavyAttacks.Length];
        for (int i = 0; i < heavyAttacks.Length; i++)
        {
            heavies[i] = heavyAttacks[i].GetComponent<MoveData>();
            heaviesStaminaCosts[i] = heavies[i].GetStaminaCost();
        }//copy heavy attack MoveData over to lights
        //Debug.Log("heavies.Length: " + heavies.Length);
        specials = new MoveData[specialAttacks.Length];
        for (int i = 0; i < specialAttacks.Length; i++)
        {
            specials[i] = specialAttacks[i].GetComponent<MoveData>();
        }//copy special attack MoveData over to lights
    }

    public void LightAttackCombo()
    {
        //Debug.Log("Light Attack Requested");
        lastAttackRequestTime += Time.time;
        if (!attacking)
        {
            StartAttacking(0);
        }
    }

    public void HeavyAttackCombo()
    {
        //Debug.Log("Heavy Attack Requested");
        lastAttackRequestTime += Time.time;
        if (!attacking)
        {
            StartAttacking(1);
        }
    }

    public void SpecialAttack()
    {
        //once specials are developed, implement logic to handle different specials
    }

    //should be called to start timer
    public void StartAttacking(int am)
    {
        //Debug.Log("Start Attacking in mode " + am);
        attacking = true;
        currentMove = 0;
        attackMode = am;
        UpdateAttackTimes();
    }

    public void StopAttacking()
    {
        attacking = false;
        currentMove = -1;
        //Debug.Log("Stopped Attacking");
    }

    public float FirstLightCost()
    {
        return lightsStaminaCosts[0];
    }

    public float FirstHeavyCost()
    {
        return heaviesStaminaCosts[0];
    }


    public void UpdateAttackTimes()
    {
        attackStartTime = Time.time;
        //Debug.Log("Updating Attack Times with attackMode "+attackMode);
        moveStartTimes.Clear();
        switch (attackMode)
        {
            case 0://LIGHT ATTACK
                //Debug.Log("hitboxProperties.Length: " + lights[currentMove].GetHitboxProperties().Length);
                for (int i = 0; i < lights[currentMove].GetHitboxProperties().Length; i++)
                {
                    moveStartTimes.Add(lights[currentMove].GetHitboxProperties()[i].GetTimeEnabled());
                    //Debug.Log("Checking if get hitbox properties get time enabled fetches value: "+lights[currentMove].GetHitboxProperties()[i].GetTimeEnabled());
                }
                lastHitboxOutTime = lights[currentMove].GetMaxLifespan();
                //Debug.Log("Max lifespan:" + lastHitboxOutTime);
                break;
            case 1://HEAVY ATTACK
                //Debug.Log("hitboxProperties.Length: " + heavies[currentMove].GetHitboxProperties().Length);
                for (int i = 0; i < heavies[currentMove].GetHitboxProperties().Length; i++)
                {
                    moveStartTimes.Add(heavies[currentMove].GetHitboxProperties()[i].GetTimeEnabled());
                    //Debug.Log("Checking if get hitbox properties get time enabled fetches value: " + heavies[currentMove].GetHitboxProperties()[i].GetTimeEnabled());
                }
                lastHitboxOutTime = heavies[currentMove].GetMaxLifespan();
                //Debug.Log("Max lifespan:" + lastHitboxOutTime);
                break;
            case 2://SPECIAL ATTACK
                //implement logic for specials later when they're actually in the game
                break;
        }
        foreach(float f in moveStartTimes)
        {
            //Debug.Log("Move Start Time: " + f);
        }
        //Debug.Log("moveStartTimes.Count: "+moveStartTimes.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if(attacking)
        {
            if (!staminaCostPaid)
            {
                if (attachedToPlayer)
                {
                    staminaCostPaid = true;
                    switch(attackMode)
                    {
                        case 0://LIGHT ATTACK
                            tpc.LoseStamina(lightsStaminaCosts[currentMove], 0, 0.0f);
                            break;
                        case 1://HEAVY ATTACK
                            tpc.LoseStamina(heaviesStaminaCosts[currentMove], 0, 0.0f);
                            break;
                        case 2://SPECIAL ATTACK
                            //implement logic for specials later when they're actually in the game
                            break;
                    }
                }
                else
                {
                    staminaCostPaid = true;
                    //implement code to 
                }
            }
            if(Time.time> attackStartTime + lastHitboxOutTime)
            {
                switch(attackMode)
                {
                    case 0://LIGHT ATTACK
                        if (currentMove < lights.Length - 1 && Time.time < lastAttackRequestTime+attackBufferWindow)
                        {
                            currentMove++;
                            staminaCostPaid = false;
                            //Debug.Log("Incrementing currentMove to " + currentMove);
                            UpdateAttackTimes();
                        }
                        else
                        {
                            StopAttacking();
                            if (attachedToPlayer)
                            {
                                tpc.SetAttackingDone();
                            }
                            else
                            {
                                //put code to tell AI that attacking is over here
                            }
                        }
                        break;
                    case 1://HEAVY ATTACK
                        if (currentMove < heavies.Length - 1 && Time.time < lastAttackRequestTime + attackBufferWindow)
                        {
                            currentMove++;
                            staminaCostPaid = false;
                            //Debug.Log("Incrementing currentMove to " + currentMove);
                            UpdateAttackTimes();
                        }
                        else
                        {
                            StopAttacking();
                            if (attachedToPlayer)
                            {
                                tpc.SetAttackingDone();
                            }
                            else
                            {
                                //put code to tell AI that attacking is over here
                            }
                        }
                        break;
                    case 2://SPECIAL ATTACK
                           //implement logic for specials later when they're actually in the game
                        break;
                }
            }
            for(int i = 0;i < moveStartTimes.Count; i++)
            {
                if (Time.time > attackStartTime +moveStartTimes.IndexOf(i)&&attacking)
                {
                    //Debug.Log("Inside of attack creation block reached with attackMode"+attackMode+", and currentMove: "+currentMove);
                    switch (attackMode)
                    {
                        case 0://LIGHT ATTACK
                            lights[currentMove].GetHitboxProperties()[i].CreateHitbox();
                            //Debug.Log("Light Move " + currentMove + ", Hitbox " + i + " created");
                            moveStartTimes.RemoveAt(i);
                            i--;
                            break;
                        case 1://HEAVY ATTACK
                            heavies[currentMove].GetHitboxProperties()[i].CreateHitbox();
                            //Debug.Log("Heavy Move " + currentMove + ", Hitbox " + i + " created");
                            moveStartTimes.RemoveAt(i);
                            i--;
                            break;
                        case 2://SPECIAL ATTACK
                               //implement logic for specials later when they're actually in the game
                            break;
                    }
                }
            }
        }
    }
}
