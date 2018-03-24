using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickUp2 : MonoBehaviour
{
    private GameObject g;
    private GameObject img;
    private GameObject bubble_box;
    private GameObject character;
    private GameObject code_snippet;
    private Text text;
    private new AudioSource audio;
    private float distance;
    private Turning turning;
    //from kinect handclose 
    public bool pickup;

    //objects flags
    bool basket;
    bool orange;
    bool coconut;
    int coconutCount;
    bool blackberry;
    int blackberryCount;
    bool win;
    int counter = 6;

    // Use this for initialization
    void Start()
    {
        distance = 2.5f;
        turning = GetComponentInParent<Turning>();
        audio = GetComponent<AudioSource>();
        bubble_box = GameObject.FindGameObjectWithTag("bubble_box");
        character = GameObject.FindGameObjectWithTag("character");
        code_snippet = GameObject.FindGameObjectWithTag("code_snippet");
        text = bubble_box.GetComponentInChildren<Text>();
        bubble_box.SetActive(false);
        code_snippet.SetActive(false);
        character.SetActive(false);
        text.enabled = false;
        pickup = true;
        basket = false;
        orange = false;
        coconut = false;
        coconutCount = 2;
        blackberry = false;
        blackberryCount = 2;
        win = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickup && !win)
        {   //Basket 
            if (CheckCloseToTag("basket", 3.5f))
            {
                g = GameObject.FindGameObjectWithTag("basket");
                img = GameObject.FindGameObjectWithTag("basket_img");
                basket = true;
                audio.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
            }
            else if ((CheckCloseToTag("coconut", distance) || CheckCloseToTag("coconut2", distance) || CheckCloseToTag("orange", distance) 
                || CheckCloseToTag("blackberry2", distance) || CheckCloseToTag("blackberry", distance))
               && !basket && !orange && !coconut && !blackberry)
            {
                StartCoroutine(ShowMessage("You have to collect the Basket first", 4));
            }
            //Coconut
            if (CheckCloseToTag("coconut", distance) && basket)
            {
                g = GameObject.FindGameObjectWithTag("coconut");
                img = GameObject.FindGameObjectWithTag("coconut_img");
                audio.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
                coconutCount--;
                if (coconutCount == 0)
                {
                    coconut = true;
                }
            }
            else if (CheckCloseToTag("coconut2", distance) && basket)
            {
                g = GameObject.FindGameObjectWithTag("coconut2");
                img = GameObject.FindGameObjectWithTag("coconut_img2");
                audio.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
                coconutCount--;
                if (coconutCount == 0)
                {
                    coconut = true;
                }
            }
            else if ((CheckCloseToTag("orange", distance) || CheckCloseToTag("blackberry", distance) || CheckCloseToTag("blackberry2", distance))
                && basket && !orange && !coconut && !blackberry)
            {
                StartCoroutine(ShowMessage("You have to collect the Coconuts first", 2));
            }
            //Orange
            if (CheckCloseToTag("orange", distance) && basket && coconut)
            {
                g = GameObject.FindGameObjectWithTag("orange");
                img = GameObject.FindGameObjectWithTag("orange_img");
                orange = true;
                audio.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
            }
            else if ((CheckCloseToTag("blackberry", distance) || CheckCloseToTag("blackberry2", distance))
               && basket && !orange && coconut && !blackberry)
            {
                StartCoroutine(ShowMessage("You have to collect the Orange first", 4));
            }
            //Blackberry
            if (CheckCloseToTag("blackberry", distance) && basket && coconut && orange)
            {
                g = GameObject.FindGameObjectWithTag("blackberry");
                img = GameObject.FindGameObjectWithTag("blackberry_img");
                audio.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
                blackberryCount--;
                if (blackberryCount == 0)
                {
                    blackberry = true;
                }
            }
            else if (CheckCloseToTag("blackberry2", distance) && basket && coconut && orange)
            {
                g = GameObject.FindGameObjectWithTag("blackberry2");
                img = GameObject.FindGameObjectWithTag("blackberry_img2");
                audio.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
                blackberryCount--;
                if (blackberryCount == 0)
                {
                    blackberry = true;
                }
            }
            if (counter == 0 && !win)
            {
                win = true;
                turning.move = false;
                StartCoroutine(ShowMessage("Congratulations you completed Second Level", 3));
            }
            if (win)
            {
                StartCoroutine(ShowCode());
                StartCoroutine(NextLevel());
            }
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
        yield return new WaitForSecondsRealtime(10);
        #pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(name: "Sequence_Quiz");
        #pragma warning restore CS0618 // Type or member is obsolete
    }
}
