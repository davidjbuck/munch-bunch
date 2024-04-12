using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI character;
    public string characterName;

    public string[] line;
    public float textSpeed;

    private int index;

    private VoiceLines vl;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        character.text = characterName;
        StartDialogue();

        vl = GetComponent<VoiceLines>();
    }

    // Update is called once per frame
    void Update()
    {
        //TAB ADDED: would increment through text even without being in dialogue
        if(textComponent.gameObject.activeInHierarchy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (textComponent.text == line[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = line[index];
                }
            }
        }
    }

    //TAB ADDED: changed to public so I can access from mission controller
    public void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
        vl.PlayCurrentVoiceLine();
    }

    IEnumerator TypeLine()
    {
        foreach (char c in line[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < line.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            vl.PlayCurrentVoiceLine();
        }

        //TAB CHANGE: commented this out
        //else gameObject.SetActive(false);
    }
}
