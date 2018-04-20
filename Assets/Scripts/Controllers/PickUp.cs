using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    private GameObject g;
    private GameObject img;
    private GameObject bubble_box;
    private GameObject character;
    private GameObject code_snippet;
    private Text text;
    private AudioSource [] sounds;
    private AudioSource collect;
    private AudioSource winning;
    private float distance;
    private Turning turning;
    //from kinect handclose 
    private bool pickup;

    //objects flags
    bool basket;
    bool orange;
    bool coconut;
    bool blackberry;
    private bool win;
    int counter = 4;

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
        text = bubble_box.GetComponentInChildren<Text>();
        bubble_box.SetActive(false);
        character.SetActive(false);
        code_snippet.SetActive(false);
        text.enabled = false;
        pickup = true;
        basket = false;
        orange = false;
        coconut = false;
        blackberry = false;
        win = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickup && !win)
        {   //Basket 
            if (CheckCloseToTag("basket", distance))
            {
                g = GameObject.FindGameObjectWithTag("basket");
                img = GameObject.FindGameObjectWithTag("basket_img");
                basket = true;
                collect.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
            }
            else if ((CheckCloseToTag("coconut", distance) || CheckCloseToTag("orange", distance) || CheckCloseToTag("blackberry", distance))
               && !basket && !orange && !coconut && !blackberry)
            {
                StartCoroutine(ShowMessage("You have to collect the Basket first", 4));
            }
            //Coconut
            if (CheckCloseToTag("coconut", distance) && basket)
            {
                g = GameObject.FindGameObjectWithTag("coconut");
                img = GameObject.FindGameObjectWithTag("coconut_img");
                coconut = true;
                collect.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
            }
            else if ((CheckCloseToTag("orange", distance) || CheckCloseToTag("blackberry", distance))
                && basket && !orange && !coconut && !blackberry)
            {
                StartCoroutine(ShowMessage("You have to collect the Coconut first", 2));
            }
            //Orange
            if (CheckCloseToTag("orange", distance) && basket && coconut)
            {
                g = GameObject.FindGameObjectWithTag("orange");
                img = GameObject.FindGameObjectWithTag("orange_img");
                orange = true;
                collect.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
            }
            else if ((CheckCloseToTag("blackberry", distance))
               && basket && !orange && coconut && !blackberry)
            {
                StartCoroutine(ShowMessage("You have to collect the Orange first", 4));
            }
            //Blackberry
            if (CheckCloseToTag("blackberry", distance) && basket && coconut && orange)
            {
                g = GameObject.FindGameObjectWithTag("blackberry");
                img = GameObject.FindGameObjectWithTag("blackberry_img");
                collect.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
            }
        }

        if (counter == 0 && !win)
        {
            win = true;
            turning.stop = true;
            StartCoroutine(PlayWinning());
            StartCoroutine(ShowMessage("Congratulations you completed Level 1", 3));
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
        Application.LoadLevel(name: "Level_2");
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
