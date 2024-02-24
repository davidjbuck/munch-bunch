using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public GameObject itemObtainedAnimation;
    public GameObject notification;
    List<string> alreadyObtainedItemsList;
    private IEnumerator animationTime;
    void Start()
    {
        alreadyObtainedItemsList = new List<string>();
        
    }
   
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Whammy");
        Debug.Log(collision.gameObject);
        
        if (collision.gameObject.CompareTag("item") && !alreadyObtainedItemsList.Contains(collision.gameObject.name))
        {
            notification.SetActive(true); //Activates the notification icon in the journal when collecting a new item
            animationTime = AnimationTimer(3.5f);
            StartCoroutine(animationTime);
            //itemObtainedAnimation.SetActive(true);
            
            alreadyObtainedItemsList.Add(collision.gameObject.name);
        }
    }

    private IEnumerator AnimationTimer(float waitTime)
    {
        itemObtainedAnimation.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        itemObtainedAnimation.SetActive(false);
    }
}
