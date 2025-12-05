using UnityEngine;
using UnityEngine.Video;

public class ONOFFTV : MonoBehaviour
{
    public bool isOn = false;

    public VideoPlayer videoPlayer;
    public Renderer screenRenderer;

    
    public MeshCollider TvMeshCollider; 

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.enabled = false; 
        }

        if (screenRenderer != null)
            screenRenderer.enabled = false;


        if (TvMeshCollider != null)
            TvMeshCollider.enabled = false;

        if (videoPlayer != null)
            videoPlayer.Stop();
    }

    public void TogglePower()
    {
        if (isOn) TurnOff();
        else TurnOn();
    }

    public void TurnOn()
    {
        isOn = true;

        if (videoPlayer != null)
        {
            videoPlayer.enabled = true;
            videoPlayer.Play();
        }

        if (screenRenderer != null)
            screenRenderer.enabled = true;

        if (videoPlayer != null)
            videoPlayer.Play();

        if (TvMeshCollider != null)
            TvMeshCollider.enabled = true;
    }

    public void TurnOff()
    {
        isOn = false;

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.enabled = false;
        }

        if (videoPlayer != null)
            videoPlayer.Stop();

        if (screenRenderer != null)
            screenRenderer.enabled = false;

        if (TvMeshCollider != null)
            TvMeshCollider.enabled = false;
    }
}