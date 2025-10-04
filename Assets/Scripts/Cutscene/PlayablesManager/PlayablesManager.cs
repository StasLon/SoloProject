using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayablesManager : MonoBehaviour
{
    public PlayableDirector dir;

    public void ChangeWrapMode()
    {
        dir.extrapolationMode = DirectorWrapMode.None;
    }

}
