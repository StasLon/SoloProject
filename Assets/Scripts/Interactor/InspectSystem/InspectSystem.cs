using UnityEngine;
using UnityEngine.UI;

public class InspectSystem : MonoBehaviour
{
    [Header("Inspection Object")]
    [SerializeField] private Transform inspectPoint;
    [SerializeField] private float rotationSpeed = 100f;

    [Header("UI")]
    [SerializeField] private GameObject uiSlot2;
    [SerializeField] private GameObject uiSlot3;

    [Header("Player Scripts to Disable")]
    [SerializeField] private FirstPersonController playerScriptsToDisable;

    private Transform currentObject;
    private Vector3 previousMousePosition;
    private bool isInspecting = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;

    private void Update()
    {
        if (!isInspecting)
            return;

        HandleRotation();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopInspection();
        }
    }

    public void StartInspection(Transform target)
    {
        if (target == null || isInspecting)
            return;

        isInspecting = true;
        currentObject = target;

        originalParent = target.parent;
        originalPosition = target.position;
        originalRotation = target.rotation;

        target.SetParent(inspectPoint);
        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.identity;

        if (uiSlot2 != null) uiSlot2.SetActive(false);
        if (uiSlot3 != null) uiSlot3.SetActive(false);

        if (playerScriptsToDisable != null)
            playerScriptsToDisable.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        previousMousePosition = Input.mousePosition;
    }

    private void StopInspection()
    {
        if (!isInspecting)
            return;

        currentObject.SetParent(originalParent);
        currentObject.position = originalPosition;
        currentObject.rotation = originalRotation;

        currentObject = null;
        isInspecting = false;

        if (uiSlot2 != null) uiSlot2.SetActive(true);
        if (uiSlot3 != null) uiSlot3.SetActive(true);

        if (playerScriptsToDisable != null)
            playerScriptsToDisable.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleRotation()
    {
        if (currentObject == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - previousMousePosition;

            float rotY = -delta.x * rotationSpeed * Time.deltaTime;

            currentObject.Rotate(Vector3.up, rotY, Space.World);

            previousMousePosition = Input.mousePosition;
        }
    }
}
