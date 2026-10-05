using UnityEngine;
using UnityEngine.Video;
using FMODUnity;

public class TVController : MonoBehaviour
{
    private GameObject screen; 
    public GameObject screenOFF;
    private VideoPlayer videoPlayer;
    private StudioEventEmitter TVsound;

    private bool isOn = false;

    void Awake()
    {
        screen = gameObject;
        videoPlayer = GetComponent<VideoPlayer>();
        TVsound = GetComponent<StudioEventEmitter>();
    }

    public void ToggleTV()
    {
       
        //screen.SetActive(isOn);
        if (isOn)
        {
            videoPlayer.Stop();
            screenOFF.SetActive(true);
            TVsound.Stop();
        }

        else
        {
            videoPlayer.Play();
            screenOFF.SetActive(false);
            TVsound.Play();
            //Debug.Log("TV " + (isOn ? "Accesa" : "Spenta"));
        }

        isOn = !isOn;

    }
}
