using UnityEngine;

public class ItemInspectInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private InspectSystem inspectSysScript;
    string IInteractable.GetDescription()
    {
        return "Осмотреть";
    }

    void IInteractable.Interact()
    {
        inspectSysScript.StartInspection(transform);
    }
}
