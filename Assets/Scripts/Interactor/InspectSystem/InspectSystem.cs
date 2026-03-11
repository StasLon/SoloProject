using UnityEngine;
using UnityEngine.UI;

public class InspectSystem : MonoBehaviour
{
    [Header("Inspection Object")]
    [SerializeField] private Transform inspectPoint; // Точка перед камерой
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

        // Выход по ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopInspection();
        }
    }

    // ===== ВЫЗЫВАЕТСЯ ИЗ ИНТЕРАКТ-СИСТЕМЫ =====
    public void StartInspection(Transform target)
    {
        if (target == null || isInspecting)
            return;

        isInspecting = true;
        currentObject = target;

        // Сохраняем состояние объекта
        originalParent = target.parent;
        originalPosition = target.position;
        originalRotation = target.rotation;

        // Перемещаем объект к точке инспекции
        target.SetParent(inspectPoint);
        target.localPosition = Vector3.zero;
        target.localRotation = Quaternion.identity;

        // UI и блокировка игрока
        if (uiSlot2 != null) uiSlot2.SetActive(false);
        if (uiSlot3 != null) uiSlot3.SetActive(false);

        if (playerScriptsToDisable != null)
            playerScriptsToDisable.enabled = false;

        // Разблокировать курсор для вращения предмета
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        previousMousePosition = Input.mousePosition;
    }

    private void StopInspection()
    {
        if (!isInspecting)
            return;

        // Восстановление объекта
        currentObject.SetParent(originalParent);
        currentObject.position = originalPosition;
        currentObject.rotation = originalRotation;

        currentObject = null;
        isInspecting = false;

        // Включаем UI
        if (uiSlot2 != null) uiSlot2.SetActive(true);
        if (uiSlot3 != null) uiSlot3.SetActive(true);

        // Разблокируем управление игроком
        if (playerScriptsToDisable != null)
            playerScriptsToDisable.enabled = true;

        // Блокируем курсор обратно
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

            // Вращаем ТОЛЬКО влево-вправо
            currentObject.Rotate(Vector3.up, rotY, Space.World);

            previousMousePosition = Input.mousePosition;
        }
    }
}
