using System.Collections;
using UnityEngine;

public class ElevatorMoveUp : MonoBehaviour
{
    public float startYPosition = 0f; // Initial y position
    public float endYPosition = 8f; // Target y position
    public float duration = 3f; // Duration of the movement

    private float timer;
    public GameObject tonyMeatball;
    public GameObject tonyElevator;
    void Start()
    {

    }
    public void startElevator()
    {
        timer = 0f;
        StartCoroutine(MoveElevator());
    }
    IEnumerator MoveElevator()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(transform.position.x, endYPosition, transform.position.z);

        while (timer < duration)
        {
            float t = timer / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
            timer += Time.deltaTime;
        }

        transform.position = endPosition; // Ensure final position is set precisely
        tonyMeatball.SetActive(true);
        tonyElevator.SetActive(false);

        Debug.Log("Movement completed.");
    }
}