using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
    public int maxHealth;
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
    public float dodgeInputWindow;
    public float dodgeStaminaCost;
    public float dodgeDuration;
    public float dodgeSpeedMultiplier;
    public float dodgeInvincibilityStart;
    public float dodgeInvincibilityDuration;
    public float dodgeEndLag;
    public bool invincible;
    public bool dodgeDown;
    private float dodgeStartTime;
    private float lastWDownTime;
    private float lastADownTime;
    private float lastSDownTime;
    private float lastDDownTime;
    //variables to control parry and block
    public float parryDuration;
    private float parryStartTime;
    public float staminaLostPerDamageBlocked;

    public float knockdownDuration;
    private float knockdownStartTime;
    public Vector3 kbVector;

    //numOfItemTypes sets size of arrays which handle buff effects and duration
    //should be entered into editor and updated whenever a new item type is added
    //and logic to handle buff effects should be added for each new item type
    public int numOfItemTypes;
    public Inventory playerInventory;
    public bool inventoryOpen;
    private bool[] itemTypeActive;
    private float[] effectEndTimes;
    private float[] effectValues;
    public bool itemBuffActive;
    public float baseCritRate;
    public float critRateChange;
    public float critRateMultiplier;
    public float maxStaminaChange;
    public float staminaRegenChange;
    public float staminaCostChange;
    public float playerSpeedChange;
    public float playerHealingPerSecond;
    private float lastTimeHealed;
    //when adding a new item, fill which buff types it has
    //0 = crit chance up, 1 = max stamina up, 2 = stamina regen up, 3 = stamina cost decrease, 
    //4 = player speed up, 5 = instant healing, 6 = healing per second
    public bool grounded;
    public bool jumping;
    public float drag;
    public float airSpeed;
    public float maxAirSpeed;
    public LayerMask groundedCheck;
    public float raycastProtrusion;
    public float jumpForce;
    public float jumpCooldown;
    private float lastJumpTime;

    private Animator ani;

    private AudioSource[] audioSources;
    public AudioMixer mix;

    public bool playerHasControl;
    public bool inCombatMode = false;

    public bool testingKitchenMode;
    public GameObject freelook;
    public Vector3 fixedCameraPosition;
    public Vector3 fixedCameraRotation;
    public GameObject fixedCamera;
    private bool fixedCam;
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
        itemTypeActive = new bool[numOfItemTypes];
        for (int i = 0; i < itemTypeActive.Length; i++)
        {
            itemTypeActive[i] = false;
        }
        effectEndTimes = new float[numOfItemTypes];
        effectValues = new float[numOfItemTypes];
        ani = playerCollider.GetComponent<Animator>();
        ani.SetBool("Walking", false);
        audioSources = GetComponents<AudioSource>();
        if(SceneManager.GetActiveScene().name == "KitchenScene" || testingKitchenMode)
        {
            player.localScale = new Vector3(15f, 15f, 15f);
            freelook.gameObject.SetActive(false);
            movementSpeed *= 7.5f;
            maxAirSpeed *= 7.5f;
            jumpForce *= 7.5f;
            Physics.gravity *= 7.5f;
            cam.enabled = false;
            GetComponent<AudioListener>().enabled = false;
            fixedCamera.SetActive(true);
            fixedCamera.transform.position = fixedCameraPosition;
            fixedCamera.transform.rotation = Quaternion.Euler(fixedCameraRotation);
            fixedCam = true;
        }
        else
        {
            fixedCam = false;
            fixedCamera.GetComponent<AudioListener>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check item durations to disable any item effects whose durations have expired
        CheckItemDurations();

        //check if regen should happen
        if (itemTypeActive[6] && (Time.time> lastTimeHealed+1.0f))
        {
            health += (int)effectValues[6];
            lastTimeHealed = Time.time;
        }

        CheckGrounded();

        //check if stamina should regen
        if (stamina < (maxStamina+maxStaminaChange) && (int)currentPlayerState==0)
        {
            if (fatigued)//if the player is fatigued
            {
                stamina += (Time.deltaTime * ((staminaRegenSpeed+staminaRegenChange) * fatiguedRegenMultiplier));//regen at fatigued speed
                if (stamina > (maxStamina+maxStaminaChange))
                {
                    stamina = (maxStamina+maxStaminaChange);//set stamina to max if it goes over
                    fatigued = false;
                }
            }
            else//if the player is not fatigued
            {
                stamina += (Time.deltaTime * (staminaRegenSpeed + staminaRegenChange));//regen at normal speed
                if (stamina > (maxStamina + maxStaminaChange))
                {
                    stamina = (maxStamina + maxStaminaChange);//set stamina to max if it goes over
                }
            }
        }else if((int)currentPlayerState==5 && Time.time > stunEndTime)//if the player is stunned and their stun time is over
        {
            currentPlayerState = PlayerState.Neutral;//set them back to neutral state
            //Debug.Log("Ending stun");
        }else if((int)currentPlayerState == 6)
        {
            if(Time.time > knockdownStartTime + knockdownDuration)
            {
                currentPlayerState = PlayerState.Neutral;//set the player back to neutral once the knockdown ends
                //Debug.Log("Ending knockback time.");
            }
            
        }
        if(lockedOn)//if the player is already locked on
        {
            try
            {
                if (Vector3.Distance(lockOnTarget.transform.position, playerCollider.transform.position) > lockOnRange)
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
            }
            catch
            {
                lockedOn = false;
                CheckLockOnCandidates();
            }
            
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
        if ((int)currentPlayerState != 7 && (int)currentPlayerState !=4 &&playerHasControl)
        {
            GetInput();//update player input variables
            UpdateRotation();//rotate model appropriately
            CheckAttacks();
        }
    }

    private void FixedUpdate()
    {
        if (fixedCam)
        {
            fixedCamera.transform.position = fixedCameraPosition;
            fixedCamera.transform.rotation = Quaternion.Euler(fixedCameraRotation);
        }
        if(currentPlayerState==PlayerState.Knockdown)
        {
            rb.AddForce(kbVector);
            //Debug.Log(kbVector);
        }
        if(jumping)
        {
            Jump();
        }
        //if the player is in neutral, parrying or blocking state
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
        if((int)currentPlayerState != 4 && currentPlayerState!=PlayerState.Knockdown)
        {
            LimitSpeed();
        }       
    }

    //rotates the player collider appropriately depending on their current state and lock on status
    public void UpdateRotation()
    {
        Vector3 viewAngle;//variable to hold result of vector math to be used to determine appropriate orientation

        if(!lockedOn)
        {//if not locked on, do vector math to determine proper orientation relative to camera
            if(fixedCam)
            {
                viewAngle = player.position - new Vector3(fixedCamera.transform.position.x, player.position.y, fixedCamera.transform.position.z);
                orientationRefObj.forward = viewAngle.normalized;
            }
            else
            {
                viewAngle = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
                orientationRefObj.forward = viewAngle.normalized;
            }            
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
        
        //code to get input for num keys 1-4, and use inventory item in slot 0-3
        //as temporary testing implementation until adding item to hotbar is implemented
        if(Input.GetKeyDown(KeyCode.Alpha1)|| Input.GetKeyDown(KeyCode.Keypad1))
        {
            //Debug.Log("1 pressed");
            if (playerInventory.GetInventoryCapacity() > 0)
            {
                ActivateItem(playerInventory.UseItem(0));
                //playerInventory.SelectItem(0);
                playerInventory.displayText();
                playerInventory.fillGUIButtons();
                
            }
        }else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (playerInventory.GetInventoryCapacity() > 1)
            {
                ActivateItem(playerInventory.UseItem(1));
                //playerInventory.SelectItem(1);
                playerInventory.displayText();
                playerInventory.fillGUIButtons();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (playerInventory.GetInventoryCapacity() > 2)
            {
                ActivateItem(playerInventory.UseItem(2));
                //playerInventory.SelectItem(2);
                playerInventory.displayText();
                playerInventory.fillGUIButtons();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (playerInventory.GetInventoryCapacity() > 3)
            {
                ActivateItem(playerInventory.UseItem(3));
                //playerInventory.SelectItem(3);
                playerInventory.displayText();
                playerInventory.fillGUIButtons();
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOpen = !inventoryOpen;
            if(inventoryOpen)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
            }
        }
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

        if(Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time < lastWDownTime + dodgeInputWindow&&grounded)
            {
                dodgeDown = true;
            }
            else
            {
                lastWDownTime = Time.time;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time < lastADownTime + dodgeInputWindow && grounded)
            {
                dodgeDown = true;
            }
            else
            {
                lastADownTime = Time.time;
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Time.time < lastSDownTime + dodgeInputWindow && grounded)
            {
                dodgeDown = true;
            }
            else
            {
                lastSDownTime = Time.time;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time < lastDDownTime + dodgeInputWindow && grounded)
            {
                dodgeDown = true;
            }
            else
            {
                lastDDownTime = Time.time;
            }
        }

        if (Input.GetButton("Jump") && grounded && Time.time>lastJumpTime+jumpCooldown && (currentPlayerState==PlayerState.Neutral||currentPlayerState == PlayerState.Parrying||currentPlayerState==PlayerState.Blocking))
        {
            jumping = true;
            lastJumpTime = Time.time;
            ani.SetBool("Walking", false);
            audioSources[0].Stop();
            ani.SetBool("Airborne", true);
        }
        else
        {
            jumping = false;
        }

        if (dodgeDown && grounded && stamina>= dodgeStaminaCost - fatigueAllowance && ((int)currentPlayerState==0|| (int)currentPlayerState == 2|| (int)currentPlayerState == 3))
        {//if the space bar is pressed, the player has enough stamina, and the player is in neutral/parrying/blocking state
            LoseStamina(dodgeStaminaCost, 0, 0.0f);//pay the stamina cost
            dodgeStartTime = Time.time;//record the dodge start time
            //Debug.Log("DODGE");
            ani.SetBool("Walking", false);
            audioSources[0].Stop();
            currentPlayerState = PlayerState.Dodge;//change player state to dodging
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
        dodgeDown = false;
    }

    //checks if the player can attack, has input an attack, and sends appropriate requests if they can
    public void CheckAttacks()
    {
        if ((int)currentPlayerState<4)//if the player is in state that they can attack out of, with 0-3 all qualifying
        {
            if (fire1Pressed && stamina >= activeMoveset.FirstLightCost() -fatigueAllowance && grounded && inCombatMode)//if the player pressed left click
            {
                if ((int)currentPlayerState != 1)
                {
                    audioSources[1].pitch = 1.6f + Random.Range(0.0f, 0.2f);
                    audioSources[1].Play();
                    ani.SetBool("BetweenPunches", true);
                    Invoke("DisableMultiPunch", 0.05f);
                    ani.SetBool("Punching", true);
                }
                //Debug.Log("Fire1 down! Sending Light Attack Request!");
                currentPlayerState = PlayerState.Attacking;//set player state to attacking
                activeMoveset.LightAttackCombo();//send a request to light attack
                ani.SetBool("Walking", false);
                audioSources[0].Stop();
            }
            else if (fire2Pressed && stamina >= activeMoveset.FirstHeavyCost() - fatigueAllowance && grounded && inCombatMode)//if the player pressed right click
            {
                if ((int)currentPlayerState != 1)
                {
                    audioSources[1].pitch = 1.6f + Random.Range(0.0f, 0.2f);
                    audioSources[1].Play();
                    ani.SetBool("BetweenPunches", true);
                    Invoke("DisableMultiPunch", 0.05f);
                    ani.SetBool("Punching", true);

                }
                //Debug.Log("Fire2 down! Sending Heavy Attack Request!");
                currentPlayerState = PlayerState.Attacking;//set player state to attacking
                activeMoveset.HeavyAttackCombo();//send a request to heavy attack
                ani.SetBool("Walking", false);
                audioSources[0].Stop();
            }
        }       
    }

    //checks if the player is stunned, and lets them move based on input if they can
    public void MovePlayer()
    {   //if the player is in neutral, parrying or blocking state
        if ((int)currentPlayerState==0 || (int)currentPlayerState == 2 || (int)currentPlayerState == 3)
        {
            
            
            if (!lockedOn)//if the player is not locked on
            {//use input and set move direction relative to the orientationRefObject
                movementDirection = orientationRefObj.forward * verticalInput + orientationRefObj.right * horizontalInput;
            }
            else//if the player is locked on
            {//use input and set movement direction based on the camera's orientation
                movementDirection = new Vector3(gameObject.transform.forward.x,0.0f, gameObject.transform.forward.z) * verticalInput + gameObject.transform.right * horizontalInput;
            }
            
            if (movementDirection.magnitude != 0)//if there is movement input and the player is not attacking
            {
                
                //rb.isKinematic = false;//enable physics to move the player
                if (grounded)
                {
                    ani.SetBool("Walking", true);
                    if (!audioSources[0].isPlaying)
                    {
                        audioSources[0].pitch = 0.85f;
                        audioSources[0].Play();
                    }
                    rb.AddForce(10f * (movementSpeed + playerSpeedChange) * movementDirection.normalized, ForceMode.Force);//add force to rb to move                    
                }
                else
                {
                    rb.AddForce(10f * (airSpeed + playerSpeedChange) * movementDirection.normalized, ForceMode.Force);//add force to rb to move
                    ani.SetBool("Walking", false);
                    if (audioSources[0].isPlaying)
                    {
                        audioSources[0].Stop();
                    }
                }
                
            }
            else//if there is no movement input or the player is attacking
            {
                //rb.isKinematic = true;//set rb to kinematic to stop sliding
                ani.SetBool("Walking", false);
                if (audioSources[0].isPlaying)
                {                    
                    audioSources[0].Stop();
                }
            }           
        }        
    }

    //called when the player is hit by an enemy hitbox
    public void TakeDamage(int damageVal, float stunTime)
    {
        health -= damageVal;//decrease health by damage value
        if(currentPlayerState!=PlayerState.Knockdown)
        {
            currentPlayerState = PlayerState.Stun;//set the player to stunned
        }        
        //rb.isKinematic = true;//set rb to kinematic to stop sliding
        stunEndTime = Time.time+stunTime;//set stun end time
        if(health<=0)
        {
            currentPlayerState = PlayerState.GameOver;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        activeMoveset.StopAttacking();//stop any further attacks from coming out
        ani.SetBool("Punching", false);
        //Debug.Log("Taking Damage: " + damageVal + ", and Stun: " + stunTime);
    }

    //called by movesetHolder to signal that the attacks have ended and restore normal control to player
    public void SetAttackingDone()
    {
        if((int)currentPlayerState==1)//if the player is attacking
        {
            currentPlayerState = PlayerState.Neutral;//set player back to neutral state
            ani.SetBool("Punching", false);
        }
    }

    //if a trigger enters the player
    public void ResolveTriggerCollision(Collider other)
    {
        //Debug.Log("Trigger detected");
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
                            knockdownStartTime = Time.time;
                            knockdownDuration = cm.GetAttackStun()*2.0f;
                            //Debug.Log("Should be taking knockback now.");
                            Vector3 offset = player.transform.position - cm.gameObject.transform.position;
                            offset = offset.normalized;
                            offset = offset * cm.GetKnockbackForce();
                            kbVector = offset;
                            //Debug.Log(offset);
                            rb.AddForce(offset);
                            break;
                    }
                }
                else
                {
                    if (attackType == 4)
                    {
                        TakeDamage(cm.GetAttackDamage(), cm.GetAttackStun());
                        currentPlayerState = PlayerState.Knockdown;
                        knockdownStartTime = Time.time;
                        knockdownDuration = cm.GetAttackStun() * 2.0f;
                        //Debug.Log("Should be taking knockback now.");
                        Vector3 offset = player.transform.position- cm.gameObject.transform.position;
                        offset = offset.normalized;
                        offset = offset * cm.GetKnockbackForce();
                        kbVector = offset;
                        //Debug.Log(offset);
                        rb.AddForce(offset);
                    }
                    else
                    {
                        TakeDamage(cm.GetAttackDamage(), cm.GetAttackStun());//call take damage
                        //Debug.Log("else got called...");
                    }                    
                }                               
            }
        }
    }

    //called whenever the player loses stamina. determines whether the player should be fatigued,
    //whether they should take damage if they are blocking, and ends combos when stamina runs out
    public void LoseStamina(float staminaCost, int damage, float stun)
    {
        if((int)currentPlayerState == 1)
        {
            if (fire2Pressed)
            {
                audioSources[1].pitch = 1.4f + Random.Range(0.0f, 0.2f);
                audioSources[1].Play();
                ani.SetBool("BetweenPunches", true);
                Invoke("DisableMultiPunch", 0.05f);
                ani.SetBool("Punching", true);
            }
            else
            {
                audioSources[1].pitch = 1.6f + Random.Range(0.0f, 0.2f);
                audioSources[1].Play();
                ani.SetBool("BetweenPunches", true);
                Invoke("DisableMultiPunch", 0.05f);
                ani.SetBool("Punching", true);
            }
        }
        if ((int)currentPlayerState == 3)//if the player is blocking
        {
            stamina -= (staminaCost-staminaCostChange);
            if (stamina < 0.0f)
            {
                TakeDamage(damage, stun);
                fatigued = true;
            }
        }
        else
        {
            stamina -= (staminaCost - staminaCostChange);
            if (stamina < 0.0f)
            {
                fatigued = true;
                if((int)currentPlayerState == 1)
                {
                    activeMoveset.StopAttacking();
                    ani.SetBool("Punching", false);
                    currentPlayerState = PlayerState.Neutral;
                }
            }
        }
    }

    public void ToggleLockOn()
    {
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
            inCombatMode = true;
            ani.SetBool("CombatMode", true);
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
            inCombatMode = false;
            ani.SetBool("CombatMode", false);
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

            //rb.isKinematic = false;//enable physics to move the player
            rb.AddForce(10f * (movementSpeed+playerSpeedChange) * dodgeSpeedMultiplier * movementDirection.normalized, ForceMode.Force);//add force to rb to move

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

    //set a given item type to active for the purpose of checking whether its buff is active
    //then use its item values to set buff
    public void ActivateItem(Item i)
    {
        for(int j = 0; j<i.itemBuffTypes.Length; j++)
        {
            itemTypeActive[i.itemBuffTypes[j]] = true;
            effectEndTimes[i.itemBuffTypes[j]] = Time.time + i.effectDuration[j];
            effectValues[i.itemBuffTypes[j]] = i.effectValue[j];
            ApplyBuffEffects(i.itemBuffTypes[j]);
        }
        
    }

    //check item durations and disable items whose durations have expired
    public void CheckItemDurations()
    {
        for(int i = 0; i < itemTypeActive.Length; i++)
        {
            if (itemTypeActive[i] && Time.time > effectEndTimes[i])
            {
                itemTypeActive[i] = false;
                switch(i)
                {
                    case 0://crit chance
                        critRateChange = 0.0f;
                        break;
                    case 1://max stam
                        maxStaminaChange = 0.0f;
                        if (stamina > maxStamina)
                        {
                            stamina = maxStamina;
                        }
                        break;
                    case 2://stam charge speed
                        staminaRegenChange = 0.0f;
                        break;
                    case 3://stam cost decrease
                        staminaCostChange = 0.0f;
                        break;
                    case 4://player speed up
                        playerSpeedChange = 0.0f;
                        break;
                    case 5://instant healing so do nothing
                        
                        break;
                    case 6://healing per second
                        playerHealingPerSecond = 0.0f;
                        break;

                }
            }
        }
    }

    public void ApplyBuffEffects(int buffType)
    {
        switch (buffType)
        {
            case 0://crit chance
                critRateChange = effectValues[buffType];
                break;
            case 1://max stam
                maxStaminaChange = effectValues[buffType];
                break;
            case 2://stam charge speed
                staminaRegenSpeed = effectValues[buffType];
                break;
            case 3://stam cost decrease
                staminaCostChange = effectValues[buffType];
                break;
            case 4://player speed up
                playerSpeedChange = effectValues[buffType];
                break;
            case 5://instant healing, so heal!
                health += (int)effectValues[buffType];
                if (health > maxHealth)
                {
                    health = maxHealth;
                }
                break;
            case 6://healing per second
                playerHealingPerSecond = effectValues[buffType];
                lastTimeHealed = Time.time;
                break;

        }
    }

    public float GetAttackIncrease()
    {
        return critRateMultiplier;
    }

    public float GetCritRate()
    {
        return baseCritRate+critRateChange;
    }

    public bool IsCrit()
    {
        float roll = Random.Range(0.0f, 1.0f);
        if(roll < (baseCritRate + critRateChange))
        {
            return true;
        }
        return false;
    }

    public void CheckGrounded()
    {
        float offset = playerCollider.GetComponent<CapsuleCollider>().height * 0.5f;
        Vector3 pt = new Vector3(playerCollider.transform.position.x, playerCollider.transform.position.y+ offset, playerCollider.transform.position.z);
        Debug.DrawLine(pt, new Vector3(pt.x, pt.y - (offset + raycastProtrusion*player.transform.localScale.y/2.0f), pt.z), Color.red, Time.deltaTime);
        bool wasGrounded = grounded;
        grounded = Physics.Raycast(pt, Vector3.down, offset + raycastProtrusion * player.transform.localScale.y / 2.0f, groundedCheck);
        if(grounded)
        {
            if(!wasGrounded)
            {
                ani.SetBool("Airborne", false);
                audioSources[2].pitch = 1.4f;
                audioSources[2].Play();
            }
            rb.drag = drag;
        }
        else
        {
            rb.drag = 0.0f;
        }
    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
        rb.AddForce(playerCollider.transform.up * jumpForce, ForceMode.Impulse);
        jumping = false;
        audioSources[2].pitch = 0.8f;
        audioSources[2].Play();
    }

    public void LimitSpeed()
    {
        Vector3 vel = new Vector3(rb.velocity.x, 0.0f,rb.velocity.z);
        if(currentPlayerState!=PlayerState.Knockdown)
        {
            if (grounded)
            {
                if (vel.magnitude > movementSpeed + playerSpeedChange)
                {
                    //Debug.Log("Limiting ground speed");
                    Vector3 newVel = vel.normalized * (movementSpeed + playerSpeedChange);
                    rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
                }
            }
            else
            {
                if (vel.magnitude > airSpeed + playerSpeedChange)
                {
                    //Debug.Log("Limiting air speed");
                    Vector3 newVel = vel.normalized * (maxAirSpeed + playerSpeedChange);
                    rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
                }
            }
        }
                
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }

    public float GetMaxStaminaChange()
    {
        return maxStaminaChange;
    }

    public bool[] GetItemTypeActive()
    {
        return itemTypeActive;
    }

    public float[] GetItemEffectValues()
    {
        return effectValues;
    }

    //sets volume of the specified group to a decibel value(which should be kept between -80 and 0)
    public void ChangeAudioMixerGroupVolume(int group, float volume)
    {
        switch(group)
        {
            case 0://master
                mix.SetFloat("MasterVol", volume);
                break;
            case 1://voices
                mix.SetFloat("VoicesVol", volume);
                break;
            case 2://music
                mix.SetFloat("MusicVol", volume);
                break;
            case 3://sfx
                mix.SetFloat("SFXVol", volume);
                break;
        }
    }

    public void TogglePlayerControl()
    {
        playerHasControl = !playerHasControl;
    }

    private void DisableMultiPunch()
    {
        ani.SetBool("BetweenPunches", false);
    }

}
