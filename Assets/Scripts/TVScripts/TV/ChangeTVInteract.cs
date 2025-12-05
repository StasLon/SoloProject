using UnityEngine;

public class TV : MonoBehaviour, IInteractable
{
    [SerializeField] private ChangeTVShow changeTVShowScript;
    public string GetDescription()
    {
        return "Сменить канал";
    }

    public void Interact()
    {
        changeTVShowScript.SwitchVideo();
    }

    
}
