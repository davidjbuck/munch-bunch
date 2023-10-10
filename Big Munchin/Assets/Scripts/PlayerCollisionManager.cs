using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
    public ThirdPersonController tpc;
    public void OnTriggerEnter(Collider other)
    {
        tpc.ResolveTriggerCollision(other);
    }
}
