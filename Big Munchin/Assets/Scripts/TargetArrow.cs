using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetArrow : MonoBehaviour
{
    //Arrow object placed on the "player" object in the "PlayerControllerCollection" object

    public Transform target; //Target of the arrow
    public GameObject targetObject; //Game object this script is attached to
    public float rotationSpeed; //Speed at which arrow rotates

    void Update()
    {
        if (!targetObject.activeInHierarchy)
        {
            targetObject.SetActive(false);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position),
            rotationSpeed * Time.deltaTime);

       
    }
}