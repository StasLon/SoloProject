using UnityEngine;

public class DontLook : MonoBehaviour
{
    [SerializeField] private Transform playerHead;
    [SerializeField] private Transform passenger;
    [SerializeField] private float maxLookDistance = 25f;

    [SerializeField] private float killAngleThreshold = 2f;
    [SerializeField] private float warningAngleThreshold = 15f;

    [SerializeField] private DarkPassenger darkPassengerScript;
    [SerializeField] private WarningManager warningManagerScript;

    public enum LookLevel { NotLooking, Warning, Kill }

    private void Update()
    {
        LookLevel level = GetLookLevel();

        switch (level)
        {
            case LookLevel.Kill:
                darkPassengerScript.KillPlayer();
                break;

            case LookLevel.Warning:
                warningManagerScript.TriggerWarning();
                break;

            case LookLevel.NotLooking:
                warningManagerScript.SwopWarning();
                break;
        }
    }

    public LookLevel GetLookLevel()
    {
        if (playerHead == null || passenger == null)
            return LookLevel.NotLooking;

        Vector3 dir = passenger.position - playerHead.position;
        float distance = dir.magnitude;

        if (distance > maxLookDistance)
            return LookLevel.NotLooking;

        dir.Normalize();
        Vector3 forward = playerHead.forward.normalized;
        float angle = Vector3.Angle(forward, dir);

        // --- Kill: Raycast без стен ---
        Ray ray = new Ray(playerHead.position, forward);
        RaycastHit hit;
        bool hitPassenger = false;

        if (Physics.Raycast(ray, out hit, maxLookDistance))
        {
            if (hit.transform == passenger)
                hitPassenger = true;

            // Стена только блокирует Kill, Warning будет ниже
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
                hitPassenger = false;
        }

        if (hitPassenger && angle <= killAngleThreshold)
            return LookLevel.Kill;

        // --- Warning: угол + проверка стены ---
        if (angle <= warningAngleThreshold)
        {
            // Луч от игрока к пассажиру
            Ray warningRay = new Ray(playerHead.position, dir);
            if (!Physics.Raycast(warningRay, distance, LayerMask.GetMask("Walls")))
            {
                // Нет стены — возвращаем Warning
                return LookLevel.Warning;
            }
        }

        return LookLevel.NotLooking;
    }
}