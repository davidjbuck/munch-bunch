using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    //Tab added this, more on lines
    public GameObject missionBoss;


    [SerializeField] private string interactText;
    public GameObject allyText;
    public GameObject Player;
    private bool interacted;
    //public GameObject interactUI;
    public void Interact(Transform interactorTransform)
    {
        interacted = true;
        Debug.Log("INTERACT");
        showText();

        int tempMissNum = missionBoss.GetComponent<missionController>().getCurrentMission();
        if (tempMissNum == 1)
        {
            missionBoss.GetComponent<missionController>().toggleVisibility(true);
            missionBoss.GetComponent<missionController>().setCurrentMission(2);
        }
        else if (tempMissNum == 2)
        {
            missionBoss.GetComponent<missionController>().toggleVisibility(true);
            missionBoss.GetComponent<missionController>().setCurrentMission(3);
        }
        // interactUI.SetActive(false);
        //ChatBubble3D.Create(transformtransform, new Vector3(-.3f, 1.7f, 0f), ChatBubble3D.IconType.Happy, "Hello There!");
    }
    public string GetInteractText()
    {
        return interactText;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public void showText()
    {
        allyText.SetActive(true);
    }
    public void hideText()
    {
        allyText.SetActive(false);
    }
    public void Update()
    {
        if (interacted)
        {
            float dist = Vector3.Distance(this.GetTransform().position, Player.transform.position);
            Vector3 playerPosition = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
            this.transform.LookAt(playerPosition);
            //Debug.Log(dist);
            if (dist > 5.5f)
            {
                hideText();
                interacted = false;
            }
        }
    }
}
