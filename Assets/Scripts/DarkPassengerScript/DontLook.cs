using UnityEngine;

public class DontLook : MonoBehaviour
{
    [SerializeField] private Transform playerHead; // Пустой объект на уровне лица
    [SerializeField] private Transform passenger;  // Темный пассажир
    [SerializeField] private float maxDistance = 100f; // Макс. расстояние для луча
    [SerializeField] private float killAngleThreshold = 5f; // Угол для убийства (градусы)
    [SerializeField] private float warningAngleThreshold = 15f; // Угол для предупреждения (градусы)
    [SerializeField] private DarkPassenger darkPassengerScript;
    [SerializeField] private WarningManager warningManagerScript;
 

    public enum LookLevel { NotLooking, Warning, Kill }

    void Update()
    {
        LookLevel level = GetLookLevel();
        switch (level)
        {
            case LookLevel.Kill:
                Debug.Log("УБИТЬ игрока!");
                darkPassengerScript.KillPlayer();
                // Тут вызов метода убийства игрока
                break;
            case LookLevel.Warning:
                Debug.Log("ПРЕДУПРЕЖДЕНИЕ! Взгляд игрока близок к Пассажиру.");
                warningManagerScript.TriggerWarning();
                // Тут можно показать UI, анимацию и т.д.
                break;
            case LookLevel.NotLooking:
                warningManagerScript.SwopWarning();
                // Скрыть UI/эффекты
                break;
        }
    }

    public LookLevel GetLookLevel()
    {
        if (playerHead == null || passenger == null)
            return LookLevel.NotLooking;

        Vector3 dirToPassenger = passenger.position - playerHead.position;
        dirToPassenger.y = 0; // игнорируем вертикаль
        dirToPassenger.Normalize();

        Vector3 forward = playerHead.forward;
        forward.y = 0; // игнорируем вертикаль
        forward.Normalize();

        // Угол в горизонтальной плоскости между взглядом игрока и направлением на пассажира
        float horizontalAngle = Vector3.Angle(forward, dirToPassenger);

        // Запускаем луч
        Ray ray = new Ray(playerHead.position, playerHead.forward);
        RaycastHit hit;

        bool hitPassenger = false;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == passenger)
                hitPassenger = true;
        }

        // Если луч попал в пассажира и угол очень маленький — убийство
        if (hitPassenger && horizontalAngle < killAngleThreshold)
            return LookLevel.Kill;

        // Если угол попадает в зону предупреждения (левая или правая сторона)
        else if (horizontalAngle < warningAngleThreshold)
            return LookLevel.Warning;

        return LookLevel.NotLooking;
    }

}
