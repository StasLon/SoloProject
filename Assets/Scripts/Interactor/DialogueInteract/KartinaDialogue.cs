using UnityEngine;

public class KartinaDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] DialogueWithOther dialogueScript;
    string IInteractable.GetDescription()
    {
        return "Осмотреть";
    }

    void IInteractable.Interact()
    {
        dialogueScript.StartDialogue();
    }

}
