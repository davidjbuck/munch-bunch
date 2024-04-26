using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meatball : MonoBehaviour
{
    public MovesetHolder[] enemyMovesets;
    MovesetHolder enemyActiveMoveset;
    private AudioSource audioSource;
    private bool soundPlayed;
    // Start is called before the first frame update
    void Start()
    {
        enemyActiveMoveset = enemyMovesets[0];
        audioSource = GetComponent<AudioSource>();
        soundPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider col)
    {
        if (!soundPlayed)
        {
            audioSource.Play();
            //Debug.Log("PLAY SPLAT");
        }
        if (col.gameObject.tag == "Player")
        {
            //Debug.Log("HIT PLAYER");
            //spawn hitbox

            enemyActiveMoveset.LightAttackCombo();
            Destroy(this);

        }
        //Debug.Log("MEATBALL HIT");
    }
}
