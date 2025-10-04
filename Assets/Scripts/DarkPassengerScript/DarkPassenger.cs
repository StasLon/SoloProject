using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPassenger : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera playerCam;
    [SerializeField] private GameObject PlayerToDelete;
    [SerializeField] private DeathScreenUI deathScreenUISpript;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if(playerCam == null)
            playerCam = Camera.main;

    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }
    private void Update()
    {
        FacePlayer(player);
    }

    public void FacePlayer(Transform player)
    {
        if (player == null) return;
        
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    public void KillPlayer()
    {
        if (PlayerToDelete != null)
            Destroy(PlayerToDelete);

        deathScreenUISpript.ShowDeathScreen();
    }

}
