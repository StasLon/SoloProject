using UnityEngine;


public class OpenDoorAfterAnima : MonoBehaviour, IInteractable
{
    [SerializeField] AnimaTaskController animaController;
    //[SerializeField] DoorScript door;
    string IInteractable.()
    {
        if (animaController.HasTalkedToAnima())
        {

        }
    }

    void IInteractable.Interact()
    {
        throw new System.NotImplementedException();
    }

    
}
