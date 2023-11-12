using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuffIcons : MonoBehaviour
{

    List<GameObject> items;
    public GameObject iconSpawner;
    public GameObject canvas;
    UIPlayerTest player;

    //Health and Stamina Bar Animators
    public Animator healthAnimator;
    public Animator staminaAnimator;

    private IEnumerator buffTimes; //Coroutine for stopping status bar animations

    void Start()
    {
        items = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    { 
        //Checks if collided object is an item and then adds item to List items
        if(col.CompareTag("healthregen") || col.CompareTag("staminaregen") || col.CompareTag("item") && !items.Contains(col.gameObject))
        {
            if (col.CompareTag("healthregen"))
            {
                // Tells health animator to begin the flashing animation, and then stop after 30 seconds
                healthAnimator.SetBool("isBuffActive", true);
                buffTimes = BuffTimer(5.0f, healthAnimator);
                StartCoroutine(buffTimes);
                //player.isHealthRegenerating = true;
            }

            if (col.CompareTag("staminaregen"))
            {
                // Tells stamina animator to begin the flashing animation, and then stop after 30 seconds
                staminaAnimator.SetBool("isBuffActive", true);
                buffTimes = BuffTimer(5.0f, staminaAnimator);
                StartCoroutine(buffTimes);
                //player.staminaRegen += .05f;
            }

            items.Add(col.gameObject);
            col.gameObject.transform.position = new Vector3(100, 100, 100);

            //Set index of item that determines its position on the HUD
            int index = items.IndexOf(col.gameObject);
            Debug.Log("Index: " + index);

            //Set the position of the buff icon below the status bars
            Vector3 newPosition = new Vector3(iconSpawner.transform.position.x + index * 100, iconSpawner.transform.position.y, 0);
            GameObject clone = Instantiate(col.gameObject, newPosition, Quaternion.identity, iconSpawner.transform);

            //Removes buff from list and then destroys its clone
            RemoveBuff(10.0f, index);
            Destroy(clone, 10.0f);
            
            //Note: The objects do not move left when other buff is removed, adjust later on
        }
    } 

    private IEnumerator BuffTimer(float waitTime, Animator statusBarAnimator)
    {
        yield return new WaitForSeconds(waitTime);
        print("Whammy");
        statusBarAnimator.SetBool("isBuffActive", false);
    }

    private IEnumerator RemoveBuff(float waitTime, int index)
    {
        yield return new WaitForSeconds(waitTime);
        print("Whammy");
        items.RemoveAt(index);
    }

}
