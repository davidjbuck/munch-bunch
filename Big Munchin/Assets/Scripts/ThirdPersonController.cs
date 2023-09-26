using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public Transform player;
    public Transform playerCollider;
    public Rigidbody rb;
    public Transform orientationRefObj;

    public float rotationSpeed;
    public float movementSpeed;
    float horizontalInput;
    float verticalInput;
    bool fire1Pressed = false;
    bool fire2Pressed = false;
    Vector3 movementDirection;
    private bool attacking = false;
    public bool stunned;
    private float stunStartTime;
    private float stunEndTime;
    public float health;
    public MovesetHolder[] movesets;
    MovesetHolder activeMoveset;

    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;
        LockCursor();
        activeMoveset = movesets[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(stunned && Time.time > stunEndTime)
        {
            stunned = false;
        }
        GetInput();
        UpdateRotation();
        CheckAttacks();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void UpdateRotation()
    {
        Vector3 viewAngle = player.position - new Vector3 (transform.position.x, player.position.y, transform.position.z);
        orientationRefObj.forward = viewAngle.normalized;

        if(!stunned )
        {
            Vector3 inputDirection = orientationRefObj.forward*verticalInput + orientationRefObj.right*horizontalInput;

            if(inputDirection != Vector3.zero )
            {
                playerCollider.forward = Vector3.Slerp(playerCollider.forward, inputDirection.normalized, Time.deltaTime*rotationSpeed);
            }
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetStunned(bool val)
    {
        stunned = val;
        stunStartTime = Time.time;
    }

    public void GetInput() {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        fire1Pressed = Input.GetButtonDown("Fire1");
        fire2Pressed = Input.GetButtonDown("Fire2");

    }

    public void CheckAttacks()
    {
        if (!stunned)
        {
            if (fire1Pressed)
            {
                Debug.Log("Fire1 down! Sending Light Attack Request!");
                attacking = true;
                activeMoveset.LightAttackCombo();
            }
            else if (fire2Pressed)
            {
                Debug.Log("Fire2 down! Sending Heavy Attack Request!");
                attacking = true;
                activeMoveset.HeavyAttackCombo();
            }
        }       
    }

    public void MovePlayer()
    {
        if(!stunned)
        {
            movementDirection = orientationRefObj.forward * verticalInput + orientationRefObj.right * horizontalInput;
            if (movementDirection.magnitude != 0 && !attacking)
            {
                rb.isKinematic = false;
                rb.AddForce(movementDirection.normalized * movementSpeed * 10f, ForceMode.Force);
            }
            else
            {
                rb.isKinematic = true;
            }           
        }        
    }

    public void TakeDamage(int damageVal, float stunTime)
    {
        health -= damageVal;
        SetStunned(true);
        stunEndTime = Time.time+stunTime;
        activeMoveset.StopAttacking();
        Debug.Log("Taking Damage: " + damageVal + ", and Stun: " + stunTime);
    }

    public void SetAttackingDone()
    {
        attacking = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hitbox")
        {
            CollisionManager cm = other.gameObject.GetComponent<CollisionManager>();
            if (cm.GetAttackTeam() == 1)//if attackTeam is 1 and other is an enemy hitbox
            {
                //don't forget to add code for blocking or checking attackType here to see outcome of attack when other combat actions are added
                //don't forget to add code for taking knockback too when hit with heavy finisher
                TakeDamage(cm.GetAttackDamage(), cm.GetAttackStun());
            }
        }
    }
}
