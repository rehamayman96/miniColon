using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickUp2 : MonoBehaviour
{
    private GameObject g;

    private GameObject img_basket;
    private GameObject img_coconut1;
    private GameObject img_coconut2;
    private GameObject img_orange;
    private GameObject img_blackberry1;
    private GameObject img_blackberry2;

    private GameObject bubble_box;
    private GameObject character;
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
    int coconutCount;
    bool blackberry;
    int blackberryCount;
    private bool win;
    int counter = 6;

    // Use this for initialization
    void Start()
    {
        distance = 2.5f;
        turning = GetComponentInParent<Turning>();
        sounds = GetComponents<AudioSource>();
        collect = sounds[0];
        winning = sounds[1];
        img_basket = GameObject.FindGameObjectWithTag("basket_img");
        img_coconut1 = GameObject.FindGameObjectWithTag("coconut_img");
        img_coconut2 = GameObject.FindGameObjectWithTag("coconut_img2");
        img_orange = GameObject.FindGameObjectWithTag("orange_img");
        img_blackberry1 = GameObject.FindGameObjectWithTag("blackberry_img");
        img_blackberry2 = GameObject.FindGameObjectWithTag("blackberry_img2");
        bubble_box = GameObject.FindGameObjectWithTag("bubble_box");
        character = GameObject.FindGameObjectWithTag("character");
        text = bubble_box.GetComponentInChildren<Text>();
        bubble_box.SetActive(false);
        character.SetActive(false);
        img_basket.SetActive(false);
        img_coconut1.SetActive(false);
        img_coconut2.SetActive(false);
        img_orange.SetActive(false);
        img_blackberry1.SetActive(false);
        img_blackberry2.SetActive(false);
        text.enabled = false;
        pickup = true;
        basket = false;
        orange = false;
        coconut = false;
        coconutCount = 2;
        blackberry = false;
        blackberryCount = 2;
        win = false;
        StartCoroutine(ShowMessage("Trace the Code",3));
    }

    // Update is called once per frame
    void Update()
    {
        if (pickup && !win)
        {   //Basket 
            if (CheckCloseToTag("basket", 3.5f))
            {
                g = GameObject.FindGameObjectWithTag("basket");
                basket = true;
                collect.Play();
                g.SetActive(false);
                img_basket.SetActive(true);
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
                #pragma warning disable CS0618 // Type or member is obsolete
                if (img_coconut1.active)
                #pragma warning restore CS0618 // Type or member is obsolete
                {
                    img_coconut2.SetActive(true);
                }
                else
                {
                    img_coconut1.SetActive(true);
                }
                g = GameObject.FindGameObjectWithTag("coconut");
                collect.Play();
                g.SetActive(false);
                counter--;
                coconutCount--;
                if (coconutCount == 0)
                {
                    coconut = true;
                }
            }
            else if (CheckCloseToTag("coconut2", distance) && basket)
            {
                #pragma warning disable CS0618 // Type or member is obsolete
                if (img_coconut1.active)
                #pragma warning restore CS0618 // Type or member is obsolete
                {
                    img_coconut2.SetActive(true);
                }
                else
                {
                    img_coconut1.SetActive(true);
                }
                g = GameObject.FindGameObjectWithTag("coconut2");
                collect.Play();
                g.SetActive(false);
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
                img_orange.SetActive(true);
                orange = true;
                collect.Play();
                g.SetActive(false);
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
                #pragma warning disable CS0618 // Type or member is obsolete
                if (img_blackberry1.active)
                 #pragma warning restore CS0618 // Type or member is obsolete
                {
                    img_blackberry2.SetActive(true);
                }
                else
                {
                    img_blackberry1.SetActive(true);
                }
                g = GameObject.FindGameObjectWithTag("blackberry");
                collect.Play();
                g.SetActive(false);
                counter--;
                blackberryCount--;
                if (blackberryCount == 0)
                {
                    blackberry = true;
                }
            }
            else if (CheckCloseToTag("blackberry2", distance) && basket && coconut && orange)
            {
                #pragma warning disable CS0618 // Type or member is obsolete
                if (img_blackberry1.active)
                #pragma warning restore CS0618 // Type or member is obsolete
                {
                    img_blackberry2.SetActive(true);
                }
                else
                {
                    img_blackberry1.SetActive(true);
                }
                g = GameObject.FindGameObjectWithTag("blackberry2");
                collect.Play();
                g.SetActive(false);
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
                turning.stop = true;
                StartCoroutine(PlayWinning());
                StartCoroutine(ShowMessage("Congratulations you completed Level 2", 3));
            }
            if (win)
            {
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

    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(10);
        #pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(name: "Video");
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
