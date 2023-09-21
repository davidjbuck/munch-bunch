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
    Vector3 movementDirection;
    public bool stunned;

    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;
        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        UpdateRotation();
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
    }

    public void GetInput() {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    public void MovePlayer()
    {
        if(!stunned)
        {
            movementDirection = orientationRefObj.forward * verticalInput + orientationRefObj.right * horizontalInput;
            if (movementDirection.magnitude != 0)
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
}
