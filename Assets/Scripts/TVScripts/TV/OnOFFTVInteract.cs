using UnityEngine;

public class OnOFFTVInteract : MonoBehaviour, IInteractable
{
    [SerializeField] public ONOFFTV OnOffTVScript;
    public string GetDescription()
    {
        return "Включить/Выключить";
    }

    public void Interact()
    {
        OnOffTVScript.TogglePower();
    }
}
