using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningManager : MonoBehaviour
{
    [SerializeField] WarningOverlayController warningOverlaySpript;
    [SerializeField] WarningTextSpawner warningTextSpawnerScript;


    public void TriggerWarning()
    {
        warningOverlaySpript.FadeIn();
        warningTextSpawnerScript.StartSpawning();
        Debug.Log("TriggerWarning");
    }

    public void SwopWarning()
    {
        warningOverlaySpript.FadeOut();
        warningTextSpawnerScript.StopSpawning();
    }


}
