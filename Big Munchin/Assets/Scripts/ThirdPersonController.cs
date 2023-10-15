using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{   //variables to handle orientation and movement
    Camera cam;
    public Transform player;
    public Transform playerCollider;
    public Rigidbody rb;
    public Transform orientationRefObj;
    public float rotationSpeed;
    public float movementSpeed;
    //input variables
    float horizontalInput;
    float verticalInput;
    bool fire1Pressed = false;
    bool fire2Pressed = false;
    bool blockPressed = false;
    bool blockHeld = false;
    Vector3 movementDirection;
    //variables to handle playerState, moves, stun, and health
    private float stunEndTime;
    public int health;
    public MovesetHolder[] movesets;
    MovesetHolder activeMoveset;
    public PlayerState currentPlayerState;
    //variables to handle stamina, regen, and fatigue
    public float maxStamina;
    public float stamina;
    public float staminaRegenSpeed;
    public float fatigueAllowance;//extra stamina that may be paid if the player is low, but makes them fatigued
    public bool fatigued;//true if the player has run out of stamina
    public float fatiguedRegenMultiplier;
    //variables to handle lock on
    public float lockOnRange;
    public bool lockedOn;
    private GameObject lockOnTarget;
    public GameObject lockOnIconsHolder;
    public GameObject lockedOnIcon;
    public GameObject lockOnCandidateIcon;
    public float lockOnIconYOffset;
    public float lockOnCandSearchPeriod;
    private float lastSearchTime;
    private bool lockOnCandidateFound = false;
    //variables to control dodge
    public float dodgeStaminaCost;
    public float dodgeDuration;
    public float dodgeSpeedMultiplier;
    public float dodgeInvincibilityStart;
    public float dodgeInvincibilityDuration;
    public float dodgeEndLag;
    public bool invincible;
    private float dodgeStartTime;
    //variables to control parry and block
    public float parryDuration;
    private float parryStartTime;
    public float staminaLostPerDamageBlocked;

    public float knockdownDuration;
    private float knockdownStartTime;
    public enum PlayerState
    {//enum to hold player state. Determines which actions can be taken,
     //controls much of the flow between different blocks of code
        Neutral=0, Attacking=1, Parrying=2, Blocking=3, Dodge=4, Stun=5, Knockdown=6, GameOver=7
    }

    // Start is called before the first frame update
    void Start()
    {   //grab components and set some initial values
        cam = GetComponent<Camera>();
        rb.freezeRotation = true;
        LockCursor();
        activeMoveset = movesets[0];
        lastSearchTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {   //check if stamina should regen
        if (stamina < maxStamina && (int)currentPlayerState==0)
        {
            if (fatigued)//if the player is fatigued
            {
                stamina += (Time.deltaTime * (staminaRegenSpeed * fatiguedRegenMultiplier));//regen at fatigued speed
                if (stamina > maxStamina)
                {
                    stamina = maxStamina;//set stamina to max if it goes over
                    fatigued = false;
                }
            }
            else//if the player is not fatigued
            {
                stamina += (Time.deltaTime * staminaRegenSpeed);//regen at normal speed
                if (stamina > maxStamina) stamina = maxStamina;//set stamina to max if it goes over
            }
        }else if((int)currentPlayerState==5 && Time.time > stunEndTime)//if the player is stunned and their stun time is over
        {
            currentPlayerState = PlayerState.Neutral;//set them back to neutral state
            //Debug.Log("Ending stun");
        }else if((int)currentPlayerState == 6 && Time.time > knockdownStartTime + knockdownDuration)
        {
            currentPlayerState = PlayerState.Neutral;//set the player back to neutral once the knockdown ends
        }
        if(lockedOn)//if the player is already locked on
        {            
            if(Vector3.Distance(lockOnTarget.transform.position,playerCollider.transform.position)>lockOnRange)
            {//if the lock on target is out of range                
                CheckLockOnCandidates();//check for new candidates
                if (lockOnCandidateFound)
                {
                    lockedOn = true;//if a candidate was found, set locked on to true
                }
                else
                {//if no candidates are found, then turn off the icons and set lockedOn to false
                    lockedOnIcon.SetActive(false);
                    lockOnCandidateIcon.SetActive(false);
                    lockedOn = false;
                }
            }//move the icon holder
            lockOnIconsHolder.transform.position = new Vector3(lockOnTarget.transform.position.x, lockOnTarget.transform.position.y + lockOnIconYOffset, lockOnTarget.transform.position.z);
        }else if(!lockedOn)//if the player was not already locked on
        {
            if (Time.time - lastSearchTime > lockOnCandSearchPeriod)//if time enough time has passed since last search
            {
                CheckLockOnCandidates();//check for lock on candidates
            }
            if (lockOnCandidateFound)//if a candidate was found
            {//update the icon position
                lockOnIconsHolder.transform.position = new Vector3(lockOnTarget.transform.position.x, lockOnTarget.transform.position.y + lockOnIconYOffset, lockOnTarget.transform.position.z);
            }
            
        }
        if ((int)currentPlayerState != 7 && (int)currentPlayerState !=4)
        {
            GetInput();//update player input variables
            UpdateRotation();//rotate model appropriately
            CheckAttacks();
        }
    }

    private void FixedUpdate()
    {   //if the player is in neutral, parrying or blocking state
        if ((int)currentPlayerState == 0||(int)currentPlayerState == 2 || (int)currentPlayerState == 3)
        {   //if parrying and parry duration is over
            if ((int)currentPlayerState == 2 && Time.time > parryStartTime + parryDuration)
            {   //set the player state to blocking
                currentPlayerState = PlayerState.Blocking;
            }
            MovePlayer();//move the player
        }else if((int)currentPlayerState == 4)
        {
            Dodge();//DODGE
        }
    }

    //rotates the player collider appropriately depending on their current state and lock on status
    public void UpdateRotation()
    {
        Vector3 viewAngle;//variable to hold result of vector math to be used to determine appropriate orientation

        if(!lockedOn)
        {//if not locked on, do vector math to determine proper orientation relative to camera
            viewAngle = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientationRefObj.forward = viewAngle.normalized;
        }
        else
        {//if locked on, do vector math to determine proper orientation relative to lock on target
            viewAngle = new Vector3(lockOnTarget.transform.position.x, player.position.y, lockOnTarget.transform.position.z) - player.position;
            orientationRefObj.forward = viewAngle.normalized;
        }

        if((int)currentPlayerState!=5 )//if the player is not stunned
        {   //vector math to determine appropriate forward and right directions
            Vector3 inputDirection = orientationRefObj.forward * verticalInput + orientationRefObj.right * horizontalInput;
            if(!lockedOn )//if the player is not locked on
            {
                if (inputDirection != Vector3.zero)//if the player has pressed down WASD and there has been movement input
                {//update the player collider rotation
                    playerCollider.forward = Vector3.Slerp(playerCollider.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
                }
            }
            else//if the player is locked on
            {
                playerCollider.forward = Vector3.Slerp(playerCollider.forward, viewAngle.normalized, Time.deltaTime * rotationSpeed);
            }
            
        }
    }

    //locks the cursor to the screen and hides it
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //unlocks the cursor and makes it visible
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //updates the states of input variables
    public void GetInput() {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        fire1Pressed = Input.GetButtonDown("Fire1");
        fire2Pressed = Input.GetButtonDown("Fire2");
        blockPressed = Input.GetKeyDown(KeyCode.LeftShift);
        blockHeld = Input.GetKey(KeyCode.LeftShift);
        if (Input.GetButtonDown("Fire3"))
        {
            //Debug.Log("Middle clicked!");
            ToggleLockOn();
        }
        if (Input.GetButtonDown("Dodge")&& stamina>= dodgeStaminaCost - fatigueAllowance && ((int)currentPlayerState==0|| (int)currentPlayerState == 2|| (int)currentPlayerState == 3))
        {//if the space bar is pressed, the player has enough stamina, and the player is in neutral/parrying/blocking state
            LoseStamina(dodgeStaminaCost, 0, 0.0f);//pay the stamina cost
            dodgeStartTime = Time.time;//record the dodge start time
            Debug.Log("DODGE");
            currentPlayerState = PlayerState.Dodge;//change player state to dodging
            if(Mathf.Abs(verticalInput)<0.1 && Mathf.Abs(horizontalInput) < 0.1)
            {
                verticalInput = -1.0f;
                horizontalInput = 0.0f;
            }
            if (!lockedOn)//if the player is not locked on
            {//use input and set move direction relative to the orientationRefObject
                movementDirection = orientationRefObj.forward * verticalInput + orientationRefObj.right * horizontalInput;
            }
            else//if the player is locked on
            {//use input and set movement direction based on the camera's orientation
                movementDirection = new Vector3(gameObject.transform.forward.x, 0.0f, gameObject.transform.forward.z) * verticalInput + gameObject.transform.right * horizontalInput;
            }
        }else if (blockPressed)
        {
            if((int)currentPlayerState == 0)
            {
                //Debug.Log("Parry Started");
                parryStartTime = Time.time;
                currentPlayerState = PlayerState.Parrying;
            }
        }else if(!blockHeld && ((int)currentPlayerState == 2 || (int)currentPlayerState == 3))
        {//if the player has let go of the block button and they are in parrying or blocking mode
            currentPlayerState = PlayerState.Neutral;
        }
    }

    //checks if the player can attack, has input an attack, and sends appropriate requests if they can
    public void CheckAttacks()
    {
        if ((int)currentPlayerState<4)//if the player is in state that they can attack out of, with 0-3 all qualifying
        {
            if (fire1Pressed && stamina >= activeMoveset.FirstLightCost() -fatigueAllowance)//if the player pressed left click
            {
                //Debug.Log("Fire1 down! Sending Light Attack Request!");
                currentPlayerState = PlayerState.Attacking;//set player state to attacking
                activeMoveset.LightAttackCombo();//send a request to light attack
            }
            else if (fire2Pressed && stamina >= activeMoveset.FirstHeavyCost() - fatigueAllowance)//if the player pressed right click
            {
                //Debug.Log("Fire2 down! Sending Heavy Attack Request!");
                currentPlayerState = PlayerState.Attacking;//set player state to attacking
                activeMoveset.HeavyAttackCombo();//send a request to heavy attack
            }
        }       
    }

    //checks if the player is stunned, and lets them move based on input if they can
    public void MovePlayer()
    {   //if the player is in neutral, parrying or blocking state
        if ((int)currentPlayerState==0 || (int)currentPlayerState == 2 || (int)currentPlayerState == 3)
        {
            if(!lockedOn)//if the player is not locked on
            {//use input and set move direction relative to the orientationRefObject
                movementDirection = orientationRefObj.forward * verticalInput + orientationRefObj.right * horizontalInput;
            }
            else//if the player is locked on
            {//use input and set movement direction based on the camera's orientation
                movementDirection = new Vector3(gameObject.transform.forward.x,0.0f, gameObject.transform.forward.z) * verticalInput + gameObject.transform.right * horizontalInput;
            }
            
            if (movementDirection.magnitude != 0)//if there is movement input and the player is not attacking
            {
                rb.isKinematic = false;//enable physics to move the player
                rb.AddForce(10f * movementSpeed * movementDirection.normalized, ForceMode.Force);//add force to rb to move
            }
            else//if there is no movement input or the player is attacking
            {
                rb.isKinematic = true;//set rb to kinematic to stop sliding
            }           
        }        
    }

    //called when the player is hit by an enemy hitbox
    public void TakeDamage(int damageVal, float stunTime)
    {
        health -= damageVal;//decrease health by damage value
        currentPlayerState = PlayerState.Stun;//set the player to stunned
        rb.isKinematic = true;//set rb to kinematic to stop sliding
        stunEndTime = Time.time+stunTime;//set stun end time
        if(health<=0)
        {
            currentPlayerState = PlayerState.GameOver;
        }
        activeMoveset.StopAttacking();//stop any further attacks from coming out
        //Debug.Log("Taking Damage: " + damageVal + ", and Stun: " + stunTime);
    }

    //called by movesetHolder to signal that the attacks have ended and restore normal control to player
    public void SetAttackingDone()
    {
        if((int)currentPlayerState==1)//if the player is attacking
        {
            currentPlayerState = PlayerState.Neutral;//set player back to neutral state
        }
    }

    //if a trigger enters the player
    public void ResolveTriggerCollision(Collider other)
    {
        Debug.Log("Trigger detected");
        if (other.gameObject.CompareTag("Hitbox") && !invincible)//if the trigger is a hitbox, and the player is not invincible
        {
            CollisionManager cm = other.gameObject.GetComponent<CollisionManager>();//get the collision manager
            if (cm.GetAttackTeam() == 1)//if attackTeam is 1 and other is an enemy hitbox
            {
                AttackProperties ap = cm.GetAttackProperties();
                int attackType = (int)ap.at;
                //don't forget to add code for blocking or checking attackType here to see outcome of attack when other combat actions are added
                if ((int)currentPlayerState ==2)
                {
                    //don't forget to add code here integrating with ai controller to make enemy actually get stunned
                }else if((int)currentPlayerState ==3)
                {
                    
                    switch(attackType)
                    {
                        case 0://light
                            LoseStamina(cm.GetAttackDamage()*staminaLostPerDamageBlocked, cm.GetAttackDamage(), cm.GetAttackStun());
                            break;
                        case 1://heavy
                            TakeDamage(cm.GetAttackDamage(), cm.GetAttackStun());
                            break;
                        case 2://special
                            //implement logic for specials here later
                            break;
                        case 3://light finisher
                            LoseStamina(cm.GetAttackDamage() * staminaLostPerDamageBlocked, cm.GetAttackDamage(), cm.GetAttackStun());
                            break;
                        case 4://heavy finisher
                            TakeDamage(cm.GetAttackDamage(), cm.GetAttackStun());
                            currentPlayerState = PlayerState.Knockdown;
                            //don't forget to actually add code here later to make knockback happen
                            break;
                    }
                }
                else
                {
                    if (attackType == 4)
                    {
                        TakeDamage(cm.GetAttackDamage(), cm.GetAttackStun());
                        currentPlayerState = PlayerState.Knockdown;
                        //don't forget to actually add code here later to make knockback happen
                    }
                    else
                    {
                        TakeDamage(cm.GetAttackDamage(), cm.GetAttackStun());//call take damage
                    }                    
                }                               
            }
        }
    }

    //called whenever the player loses stamina. determines whether the player should be fatigued,
    //whether they should take damage if they are blocking, and ends combos when stamina runs out
    public void LoseStamina(float staminaCost, int damage, float stun)
    {
        if ((int)currentPlayerState == 3)//if the player is blocking
        {
            stamina -= staminaCost;
            if (stamina < 0.0f)
            {
                TakeDamage(damage, stun);
                fatigued = true;
            }
        }
        else
        {
            stamina -= staminaCost;
            if (stamina < 0.0f)
            {
                fatigued = true;
                if((int)currentPlayerState == 1)
                {
                    activeMoveset.StopAttacking();
                    currentPlayerState = PlayerState.Neutral;
                }
            }
        }
    }

    public void ToggleLockOn()
    {
        //update this method later to check if new targets are in range and switch lock down target when an enemy is defeated
        lockedOn = !lockedOn;
        if(lockedOn)
        {
            CheckLockOnCandidates();
        }
    }

    public void CheckLockOnCandidates()
    {
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("LockOnTarget");
        float minDiff = 1;
        int currentMinIndex = -1;
        for (int i = 0; i < potentialTargets.Length; i++)
        {
            Vector3 screenSpace = cam.WorldToViewportPoint(potentialTargets[i].transform.position);
            screenSpace -= new Vector3(0.5f, 0.5f, 0.0f);
            screenSpace = new Vector3(screenSpace.x, screenSpace.y, 0.0f);
            float diff = Mathf.Abs(Vector3.Magnitude(screenSpace));
            //Debug.Log(i + ": " + diff);
            if (diff < minDiff && Mathf.Abs(Vector3.Distance(playerCollider.transform.position, potentialTargets[i].transform.position)) < lockOnRange)
            {
                currentMinIndex = i;
                minDiff = diff;
            }
        }
        if (currentMinIndex != -1)
        {
            lockOnTarget = potentialTargets[currentMinIndex];
            lockOnCandidateFound = true;
            if(lockedOn)
            {
                lockedOnIcon.SetActive(true);
                lockOnCandidateIcon.SetActive(false);
            }
            else
            {
                lockedOnIcon.SetActive(false);
                lockOnCandidateIcon.SetActive(true);
            }
            //Debug.Log(currentMinIndex + " selected");
        }
        else
        {
            //Debug.Log("No suitable lock on targets found");
            lockOnCandidateFound = false;
            lockedOnIcon.SetActive(false);
            lockOnCandidateIcon.SetActive(false);
        }
    }

    public void Dodge()
    {
        float t = Time.time;
        if (t < dodgeStartTime+dodgeDuration)
        {

            rb.isKinematic = false;//enable physics to move the player
            rb.AddForce(10f * movementSpeed * dodgeSpeedMultiplier * movementDirection.normalized, ForceMode.Force);//add force to rb to move

            if (t>dodgeStartTime+dodgeInvincibilityStart && t<dodgeStartTime+ dodgeInvincibilityStart + dodgeInvincibilityDuration)
            {
                invincible = true;
            }
            else
            {
                invincible = false;
            }
        }
        else if(t<dodgeStartTime+dodgeDuration+dodgeEndLag)
        {
            invincible = false;
        }
        else
        {
            currentPlayerState = PlayerState.Neutral;
        }
    }//why do I hear Piccolo noises
}
