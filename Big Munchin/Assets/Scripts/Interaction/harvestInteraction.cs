using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class harvestInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactText;
    [SerializeField] private string hName;

    private bool interacted;
    public GameObject Player;
    public GameObject MC;


    public void Interact(Transform interactorTransform)
    {
        interacted = true;
        missionCheck();
        harvest();
    }
    public string GetInteractText()
    {
        return interactText + " " + hName;
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public void harvest()
    {
        int amountInstant = Random.Range(1, 4);
        Destroy(gameObject);
        for (int x = 0; x < amountInstant; x++)
        {
            Instantiate(Resources.Load(hName), transform.position, Quaternion.identity);
        }
        Instantiate(Resources.Load(gameObject.name + "_Harvested"), transform.position, transform.rotation);
    }

    public void missionCheck()
    {
        int tempMissNumb = MC.GetComponent<missionController>().getCurrentSideMission();
        if (tempMissNumb == 0)
        {
            //this works for a harvesting side mission
            //MC.GetComponent<missionController>().sMissionZeroFunction();
        }
    }

    public void Update()
    {
        if (interacted)
        {
            float dist = Vector3.Distance(this.GetTransform().position, Player.transform.position);
            Vector3 playerPosition = new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
            if (dist > 5.5f)
            {
                interacted = false;
            }
        }
    }
}
