using UnityEngine;

public class ItemInteractNotQuestble : MonoBehaviour, IInteractable
{
    [SerializeField] public InspectSystem inspectSysScript;
    
    string IInteractable.GetDescription()
    {
        return "Осмотреть";
    }

    void IInteractable.Interact()
    {
        inspectSysScript.StartInspection(transform);
    }
}
