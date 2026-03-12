using UnityEngine;
using UnityEngine.Video;

public class ChangeTVShow : MonoBehaviour
{
    
    public VideoClip[] clips;

    private VideoPlayer videoPlayer;
    private int currentIndex = 0;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer == null)
        {
            return;
        }

        if (clips.Length > 0)
        {
            videoPlayer.clip = clips[0];
            videoPlayer.Play();
        }
    }

    public void SwitchVideo()
    {
        if (clips.Length == 0 || videoPlayer == null) return;

        currentIndex = (currentIndex + 1) % clips.Length; 
        videoPlayer.clip = clips[currentIndex];
        videoPlayer.Play();
    }


}
