using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CamerasPriorities : MonoBehaviour
{

    [SerializeField] private Camera CutSceneCamera;
    [SerializeField] private Camera PlayerCamera;

    public void TurnOnCamera()
    {
        PlayerCamera.gameObject.SetActive(true);
    }

    public void TurnOffCamera()
    {
        PlayerCamera.gameObject.SetActive(false);
    }
}
