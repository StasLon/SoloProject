using UnityEngine;

public class KeypadInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private Keypad KeypadScript;
    
    public void Interact()
    {
      KeypadScript.EnterKeypad();
      
    }

    public string GetDescription()
    {
        return "Взаимодействовать";
    }

    
}
