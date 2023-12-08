using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public static bool PlayerReachedEnd = false;
    public void endDemo()
    {
        if (TonyRunAI.TonyReachedEnd)
        {
            SceneManager.LoadScene("End Screen");

        }
    }
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.tag);

        if (col.tag == "Player")
        {
            PlayerReachedEnd = true;
            endDemo();
        }
    }
}
