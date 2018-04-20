using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickUp5 : MonoBehaviour {

    private GameObject g;
    private GameObject img;
    private GameObject bubble_box;
    private GameObject character;
    private GameObject code_snippet;
    private GameObject c;
    private Text text;
    private AudioSource[] sounds;
    private AudioSource collect;
    private AudioSource winning;
    private float distance;
    private Turning turning;
    private Text c_count;
    private Text b_count;
    //from kinect handclose 
    private bool pickup;

    //objects flags
    bool coconut;
    private bool win;
    int coconut_count = 4;


    // Use this for initialization
    void Start()
    {
        distance = 3f;
        turning = GetComponentInParent<Turning>();
        sounds = GetComponents<AudioSource>();
        collect = sounds[0];
        winning = sounds[1];
        bubble_box = GameObject.FindGameObjectWithTag("bubble_box");
        character = GameObject.FindGameObjectWithTag("character");
        code_snippet = GameObject.FindGameObjectWithTag("code_snippet");
        c = GameObject.FindGameObjectWithTag("coconut_count");
        text = bubble_box.GetComponentInChildren<Text>();
        c_count = c.GetComponentInChildren<Text>();
        bubble_box.SetActive(false);
        character.SetActive(false);
        code_snippet.SetActive(false);
        text.enabled = false;
        coconut = false;
        win = false;
        pickup = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickup && !win)
        {
            //Coconut
            if (coconut_count > 0 && !coconut)
            {
                if (CheckCloseToTag("coconut", distance))
                {
                    g = GameObject.FindGameObjectWithTag("coconut");
                    collect.Play();
                    g.SetActive(false);
                    c_count.text = --coconut_count + "";
                }
                else if (CheckCloseToTag("c1", distance))
                {
                    g = GameObject.FindGameObjectWithTag("c1");
                    collect.Play();
                    g.SetActive(false);
                    c_count.text = --coconut_count + "";
                }
                else if (CheckCloseToTag("c2", distance))
                {
                    g = GameObject.FindGameObjectWithTag("c2");
                    collect.Play();
                    g.SetActive(false);
                    c_count.text = --coconut_count + "";
                }
                else if (CheckCloseToTag("c3", distance))
                {
                    g = GameObject.FindGameObjectWithTag("c3");
                    collect.Play();
                    g.SetActive(false);
                    c_count.text = --coconut_count + "";
                }
                else if (CheckCloseToTag("blackberry", distance))
                {
                    g = GameObject.FindGameObjectWithTag("blackberry");
                    collect.Play();
                    g.SetActive(false);
                    c_count.text = --coconut_count + "";
                }
                else if (CheckCloseToTag("b1", distance))
                {
                    g = GameObject.FindGameObjectWithTag("b1");
                    collect.Play();
                    g.SetActive(false);
                    c_count.text = --coconut_count + "";
                }
                else if (CheckCloseToTag("b2", distance))
                {
                    g = GameObject.FindGameObjectWithTag("b2");
                    collect.Play();
                    g.SetActive(false);
                    c_count.text = --coconut_count + "";
                }

            }
            else if (coconut_count == 0 && !coconut)
            {
                img = GameObject.FindGameObjectWithTag("coconut_img");
                img.SetActive(false);
                coconut = true;
            }
        }

        if (coconut_count == 0  && !win)
        {
            win = true;
            turning.stop = true;
            StartCoroutine(PlayWinning());
            StartCoroutine(ShowMessage("Congratulations you completed Level 5", 3));
        }
        if (win)
        {
            StartCoroutine(ShowCode());
            StartCoroutine(NextLevel());
        }
    }

    bool CheckCloseToTag(string tag, float minimumDistance)
    {
        GameObject[] goWithTag = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < goWithTag.Length; ++i)
        {
            if (Vector3.Distance(transform.position, goWithTag[i].transform.position) <= minimumDistance)
                return true;
        }
        return false;
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
    }

    IEnumerator ShowCode()
    {
        yield return new WaitForSecondsRealtime(4);
        code_snippet.SetActive(true);
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(4);
        #pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(name: "Level_6");
        #pragma warning restore CS0618 // Type or member is obsolete
    }

    IEnumerator PlayWinning()
    {
        yield return new WaitForSecondsRealtime(2);
        winning.Play();
    }

    public void Go()
    {
        StartCoroutine(NextLevel());
    }
}
