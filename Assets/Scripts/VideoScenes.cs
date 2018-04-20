using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoScenes : MonoBehaviour {

    private UnityEngine.Video.VideoPlayer video;
    private GameObject controller;
    public int level;
    private string level_name;

    void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("Finish");
        video = controller.GetComponent<UnityEngine.Video.VideoPlayer>();
        video.Play();
        StartCoroutine("WaitForMovieEnd");
    }

    private void Start()
    {
        switch (level)
        {
            case 1: level_name = "Sequence_Quiz"; break;
            case 2: level_name = "IfCondition_Quiz"; break;

        }
    }

    IEnumerator WaitForMovieEnd()
    {
        while (video.isPlaying) // while the movie is playing
        {
            yield return new WaitForEndOfFrame();
        }
        // after movie is not playing / has stopped.
        OnMovieEnded();
    }

    void OnMovieEnded()
    {
        #pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(name: level_name);
        #pragma warning restore CS0618 // Type or member is obsolete
    }
}
