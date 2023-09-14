using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tabTestMove : MonoBehaviour
{
    //player stats
    private float speed = 10;
    private float turnSpeed = 100;

    void FixedUpdate()
    {
        //controls the player movement and rotation
        float move = Input.GetAxis("Vertical") * speed;
        float turn = Input.GetAxis("Horizontal") * turnSpeed;

        move *= Time.deltaTime;
        turn *= Time.deltaTime;

        transform.Translate(0, 0, move);

        transform.Rotate(0, turn, 0);
    }
}
