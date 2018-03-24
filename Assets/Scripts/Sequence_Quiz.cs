using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Sequence_Quiz : MonoBehaviour {

    private UnityEngine.Video.VideoPlayer video;
    private GameObject controller;
    private bool video_off;
    private float video_time;
    private bool message;
    public int choice;
    private GameObject bubble_box;
    private GameObject character;
    private Text text;
    private GameObject code_snippet;
    private GameObject code_snippet2;
    private GameObject code_snippet3;

    // Use this for initialization
    void Start () {
        controller = GameObject.FindGameObjectWithTag("Finish");
        video = controller.GetComponent<UnityEngine.Video.VideoPlayer>();
        video_off = false;
        message = false;
        choice = 0;
        bubble_box = GameObject.FindGameObjectWithTag("bubble_box");
        character = GameObject.FindGameObjectWithTag("character");
        text = bubble_box.GetComponentInChildren<Text>();
        code_snippet = GameObject.FindGameObjectWithTag("code_snippet");
        code_snippet2 = GameObject.FindGameObjectWithTag("code_snippet2");
        code_snippet3 = GameObject.FindGameObjectWithTag("code_snippet3");
        code_snippet.SetActive(false);
        code_snippet2.SetActive(false);
        code_snippet3.SetActive(false);
        bubble_box.SetActive(false);
        character.SetActive(false);
        text.enabled = false;
        video_time = video.frameCount/video.frameRate;
    }
	
	// Update is called once per frame
	void Update () {
        if (video_off && !message)
        {
            
            StartCoroutine(ShowMessage("Choose the corresponding code snippet",5));
            
        }
        else if( video_off && message)
        {
            //show choices
            code_snippet.SetActive(true);
            code_snippet2.SetActive(true);
            code_snippet3.SetActive(true);
            //Switch on choices
            switch (choice)
            {
                case 1: StartCoroutine(ShowMessage("Think again", 5)); choice = 0; break;
                case 2: StartCoroutine(ShowMessage("Perfect you did it", 5)); choice = 4; break;
                case 3: StartCoroutine(ShowMessage("Think again", 5)); choice = 0; break;
                case 4: StartCoroutine(NextLevel()); break;
            }

        }
        else
        {
            StartCoroutine(VideoFinished(video_time));
        }
	}

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            choice = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            choice = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            choice = 3;
        }
    }

    IEnumerator VideoFinished(double duration)
    {
        float d = (float) duration;
        yield return new WaitForSecondsRealtime(d);
        video_off = true;
        controller.SetActive(false);

    }

    IEnumerator ShowMessage(string message, float delay)
    {
        text.text = message;
        bubble_box.SetActive(true);
        character.SetActive(true);
        text.enabled = true;
        yield return new WaitForSecondsRealtime(delay);
        bubble_box.SetActive(false);
        character.SetActive(false);
        text.enabled = false;
        this.message = true;
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(8);
        #pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(name: "level_2");
        #pragma warning restore CS0618 // Type or member is obsolete
    }
}
