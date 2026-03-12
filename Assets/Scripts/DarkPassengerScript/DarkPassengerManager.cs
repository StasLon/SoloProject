using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DarkPassengerManager : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform player;
    [SerializeField] private float minCoolDown = 10f;
    [SerializeField] private float maxCoolDown = 30f;
    [SerializeField] private float teleportDistance = 5f;
    [SerializeField] private DarkPassenger passengerScript;
    [SerializeField] private FirstPersonController playerController;

    private float coolDownTimer = 1;

    private void Start()
    {
        SetRandomCoolDown();
    }
    private void Update()
    {
            
        coolDownTimer -= Time.deltaTime;

        if (coolDownTimer <= 0f)
        {
            TeleportDarkPassengerBehindPlayer();
        }
    }


    private void TeleportDarkPassengerBehindPlayer()
    {
        Vector3 offset = -player.forward * teleportDistance;
        Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        Vector3 desiredXZ = player.position + offset + randomOffset;

       
        Vector3? groundPoint = FindGroundPoint(desiredXZ);

        if (!groundPoint.HasValue)
        {
            Debug.LogWarning("Телепорт ОТМЕНЁН! Под ногами нет слоя Ground!");
            SetRandomCoolDown(); 
            return;
        }

        
        Vector3 finalPos = new Vector3(groundPoint.Value.x, -1.002f, groundPoint.Value.z);

        passengerScript.transform.position = finalPos;
        passengerScript.FacePlayer(player);

        Debug.Log($"Телепорт ЗА СПИНУ УСПЕШНО! Позиция: {finalPos}");
        SetRandomCoolDown();
    }

    
    private Vector3? FindGroundPoint(Vector3 desiredXZ)
    {
        int groundLayer = LayerMask.GetMask("Ground");

        Ray ray = new Ray(desiredXZ + Vector3.up * 15f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 30f, groundLayer))
        {
            return hit.point; 
        }

        return null;
    }
    private void SetRandomCoolDown()
    {
        coolDownTimer = Random.Range(minCoolDown, maxCoolDown);
    }


}
