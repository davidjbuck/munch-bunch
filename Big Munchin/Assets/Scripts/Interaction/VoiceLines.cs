using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLines : MonoBehaviour
{
    int index;
    AudioSource[] lines;//audio sources should be children of a voiceLineHolder game object,
    //which should be the child of the game object this VoiceLines script is attached to,
    //since GetComponentsInChildren is used to find voiceLines
    //to prevent this from incorrectly finding other sound effects, no other audio sources should be children
    //of the game object this is attached to
    
    void Start()
    {
        lines = GetComponentsInChildren<AudioSource>();
        ResetVoiceLines();
    }

    //plays the current voice line. should be called by dialog whenever a dialog option has a voice line
    //advances the current index after playing the current voice line
    //also checks to see if there is a previous voice line playing, and stops it if it is so they don't overlap
    public void PlayCurrentVoiceLine()
    {
        if(index<lines.Length)//to prevent voice lines from going out of bounds
        {
            if (index != 0)//if there is a previous voice line
            {
                if (lines[index-1].isPlaying)//check if previous voice line is playing
                {
                    lines[index-1].Stop();//and stop it so they don't overlap
                }
            }
            lines[index].Play();//play the current voice line
            index++;//and increase index
        }
        else//index is out of bounds
        {
            Debug.Log(index + " is out of bounds for voice lines");
        }
    }

    //the method used to initilaize voice lines, and reset index to 0.
    //should be called when dialog is initiated, but must be called before PlayCurrentVoiceLine is called by dialog
    public void ResetVoiceLines()
    {
        index = 0;
    }

    //test code. can be placed in a script attached to parent game object to play voice lines

    //test code to play voice lines
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            VoiceLines vl = GetComponentInChildren<VoiceLines>();
            vl.PlayCurrentVoiceLine();
        }

        //test code to reset voice lines
        if (Input.GetKeyDown(KeyCode.R))
        {
            VoiceLines vl = GetComponentInChildren<VoiceLines>();
            vl.ResetVoiceLines();
        }*/
}
