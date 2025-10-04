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

    /*private bool IsPlayerMoving() на доработке суун йопта
    {
       return playerController != null && playerController.walkSpeed < 0.1f;
    }*/
    private void TeleportDarkPassengerBehindPlayer()
    {
        Vector3 offset = -player.forward * teleportDistance;
        Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        Vector3 newPos = player.position + offset + randomOffset;

        passengerScript.transform.position = new Vector3(newPos.x, transform.position.y, newPos.z);
        passengerScript.FacePlayer(player);
        SetRandomCoolDown();
    }
    private void SetRandomCoolDown()
    {
        coolDownTimer = Random.Range(minCoolDown, maxCoolDown);
    }


}
