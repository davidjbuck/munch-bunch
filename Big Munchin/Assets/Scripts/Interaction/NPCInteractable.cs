using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    //Tab added this, more on lines 23-34
    public GameObject missionBoss;


    [SerializeField] private string interactText;
    
    //TAB ADDED gives NPCs different names to easier move missions
    [SerializeField] private string NPCName;


    public GameObject allyText;
    public GameObject Player;
    private bool interacted;
    private Animator ani;
    private bool talking = false;
    private float lastIdleChangeTime;
    private float idleChangePeriod = 10.0f;
    private float accentBehaviorPeriod = 3.0f;
    //public GameObject interactUI;
    public void Interact(Transform interactorTransform)
    {
        interacted = true;
        //Debug.Log("INTERACT");
        showText();



        //if you just defeated the goons (mission 1) and interact with the detective, switch it to mission 2
        int tempMissNum = missionBoss.GetComponent<missionController>().getCurrentMission();
        if (tempMissNum == 1 && NPCName == "Detective")
        {
            missionBoss.GetComponent<missionController>().toggleVisibility(true);
            missionBoss.GetComponent<missionController>().setCurrentMission(2);
        }
        else if (tempMissNum == 2 && NPCName != "Detective")
        {
            //otherwise, its mission 2, add to the vendor interaction count
            missionBoss.GetComponent <missionController>().missionTwoFunction();
        }
        else if (tempMissNum == 3 && NPCName == "Detective")
        {
            //otherwise if its the detective on the return, set mission 4
            missionBoss.GetComponent<missionController>().setCurrentMission(4);
        }

        int tempSMNum = missionBoss.GetComponent<missionController>().getCurrentSideMission();
        if (NPCName == "SMHarvestPlants" && tempSMNum != 1)
        {
            missionBoss.GetComponent<missionController>().setCurrentSideMission(0);
        }
        if (NPCName == "SMHarvestPlants" && tempSMNum == 1)
        {
            missionBoss.GetComponent<missionController>().toggleVisibilitySM(false);
        }
        //interactUI.SetActive(false);
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
        if (NPCName == "Detective")
        {
            talking = true;
            ani.SetBool("Talking", true);
        }
    }
    public void hideText()
    {
        allyText.SetActive(false);
        talking = false;
        ani.SetBool("Talking", false);
        SetRandomIdle();
    }

    public void Start()
    {
        ani = GetComponent<Animator>();
        float desyncTime = Random.Range(0.0f, 10.0f);
        lastIdleChangeTime = Time.time + desyncTime;
    }
    public void Update()
    {
        if (interacted)
        {
            float dist = Vector3.Distance(this.GetTransform().position, Player.transform.position);
            Vector3 playerPosition = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
            //this.transform.LookAt(playerPosition);
            //Debug.Log(dist);
            if (dist > 5.5f)
            {
                hideText();
                interacted = false;
            }
        }
        if(Time.time - lastIdleChangeTime > idleChangePeriod)
        {
            SetRandomIdle();
        }
        if(Time.time-lastIdleChangeTime > accentBehaviorPeriod)
        {
            ani.SetBool("Waving", false);
            ani.SetBool("Calling", false);
            Debug.Log("Returning to idle.");
        }
    }

    public void SetRandomIdle()
    {
        if (!talking)
        {
            lastIdleChangeTime = Time.time;
            int num = Random.Range(0, 2);
            
            switch (num)
            {
                case 0://wave
                    ani.SetBool("Waving", true);
                    ani.SetBool("Calling", false);
                    Debug.Log("Waving");
                    break;
                case 1://call
                    ani.SetBool("Waving", false);
                    ani.SetBool("Calling", true);
                    Debug.Log("Calling");
                    break;
            }
        }
    }
}