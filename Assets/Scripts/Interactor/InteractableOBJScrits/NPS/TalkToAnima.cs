using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToAnima : MonoBehaviour,IInteractable
{
    [SerializeField] Dialogue dialogueSystemScript;
    [SerializeField] AnimaTaskController animaTaskScript;
    public void Interact()
    {
        dialogueSystemScript.StartDialogue();
        animaTaskScript.AddFromAnima();
    }

    public string GetDescription()
    {
        return "Поговорить";
    }

    
}
