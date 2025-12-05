using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkToAnima : MonoBehaviour,IInteractable
{
    [SerializeField] Dialogue dialogueSystemScript;
    public void Interact()
    {
        dialogueSystemScript.StartDialogue();
    }

    public string GetDescription()
    {
        return "Поговорить";
    }

    
}
