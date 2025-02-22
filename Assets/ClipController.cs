using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClipController : MonoBehaviour
{

    public GameObject dialogueBubble;

    public TMP_Text dialogueText;

    private Coroutine dialogueCoroutine;
    // Start is called before the first frame update
    void Start()
    {
       // ShowDialogue("clip_start");
        GetComponentInChildren<ClipAnimationController>().PlayStart();
    }

    IEnumerator showDialogue(DialogueInfo info, float time = 10)
    {
        dialogueBubble.SetActive(true);
        dialogueText.text = info.text;
        yield return null;
        // if (time > 0)
        // {
        //     yield return new WaitForSeconds(time);
        //     dialogueBubble.SetActive(false);
        // }
    }

    public void ShowDialogue(string key,float time = 10)
    {
        var dialogueInfo = CSVLoader.Instance.DialogueInfoMapById[key];
        dialogueCoroutine = StartCoroutine(showDialogue(dialogueInfo,time));
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
