using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneCamera : MonoBehaviour
{
    public GameObject cutsceneCamera;
    public GameObject gameplayCamera;

    public void EndCutscene()
    {
        cutsceneCamera.SetActive(false);
        gameplayCamera.SetActive(true);
    }

    
}

