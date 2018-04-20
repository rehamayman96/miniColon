using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuizScenes: MonoBehaviour {

    private float video_time;
    private bool message;
    public int choice;
    public bool wait;
    private GameObject bubble_box;
    private GameObject character;
    private Text text;
    private GameObject code_snippet;
    private GameObject code_snippet2;
    private GameObject code_snippet3;
    private new AudioSource audio;
    public int level;
    private string level_name;
    // Use this for initialization
    void Start () {
        wait = false;
        message = false;
        choice = 0;
        audio = GetComponent<AudioSource>();
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
        switch (level)
        {
            case 3:level_name = "Level_3"; break;
            
        }
    }

    // Update is called once per frame
    void Update () {
        if(message)
        {
            //show choices
            code_snippet.SetActive(true);
            code_snippet2.SetActive(true);
            code_snippet3.SetActive(true);
            if (level == 3)
            {
                //Switch on choices
                switch (choice)
                {
                    case 1: StartCoroutine(ShowMessage("Think again", 5)); choice = 0; break;
                    case 2: StartCoroutine(ShowMessage("Perfect you did it", 5)); audio.Play(); choice = 4; break; // Opened
                    case 3: StartCoroutine(ShowMessage("Think again", 5)); choice = 0; break;
                    case 4: StartCoroutine(NextLevel()); break;
                }
            }else if (level == 5)
            {
                //Switch on choices
                switch (choice)
                {
                    case 1: StartCoroutine(ShowMessage("Perfect you did it", 5)); audio.Play(); choice = 4; break; // closed
                    case 2: StartCoroutine(ShowMessage("Think again", 5)); choice = 0; break;
                    case 3: StartCoroutine(ShowMessage("Think again", 5)); choice = 0; break;
                    case 4: StartCoroutine(NextLevel()); break;
                }
            }
        }else
            {
                StartCoroutine(ShowMessage("Choose the corresponding code snippet", 5));
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

    IEnumerator ShowMessage(string message, float delay)
    {
        wait = true;
        text.text = message;
        bubble_box.SetActive(true);
        character.SetActive(true);
        text.enabled = true;
        yield return new WaitForSecondsRealtime(delay);
        bubble_box.SetActive(false);
        character.SetActive(false);
        text.enabled = false;
        this.message = true;
        wait = false;
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(7);
        #pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(name: level_name);
        #pragma warning restore CS0618 // Type or member is obsolete
    }
}
