using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] GameObject InteractionUI;
    [SerializeField] public float interactionDistance = 2f;
    [SerializeField] TextMeshProUGUI interactionText;


    private void Update()
    {
        InteractionRay();
    }

    private void InteractionRay()
    {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one / 2f);
        RaycastHit hit;
        bool hitSomething = false;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                hitSomething = true;
                interactionText.text = interactable.GetDescription();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
        }
            }

            InteractionUI.SetActive(hitSomething);

    }
}
