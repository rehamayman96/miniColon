using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickUp4 : MonoBehaviour
{
    private GameObject g;
    private GameObject mobile;
    private GameObject rope;
    private GameObject flashlight;
    private GameObject bubble_box;
    private GameObject character;
    private GameObject back_img;
    private GameObject brown_img;
    private GameObject red_img;
    private GameObject black_img;
    private GameObject top;

    private Text text;
    private AudioSource[] sounds;
    private AudioSource collect;
    private AudioSource winning;
    private float distance;
    private Turning turning;
    private ChestAnimition chestAnim;
    //from kinect handclose 
    private bool pickup;
    //objects flags
    bool backpack;
    bool red;
    bool brown;
    bool black;
    private bool win;
    int counter = 4;

    // Use this for initialization
    void Start()
    {
        distance = 3.5f;
        turning = GetComponentInParent<Turning>();
        sounds = GetComponents<AudioSource>();
        collect = sounds[0];
        winning = sounds[1];
        back_img = GameObject.FindGameObjectWithTag("basket_img");
        brown_img = GameObject.FindGameObjectWithTag("brown_img");
        red_img = GameObject.FindGameObjectWithTag("red_img");
        black_img = GameObject.FindGameObjectWithTag("black_img");
        bubble_box = GameObject.FindGameObjectWithTag("bubble_box");
        character = GameObject.FindGameObjectWithTag("character");
        mobile = GameObject.FindGameObjectWithTag("mobile");
        rope = GameObject.FindGameObjectWithTag("rope");
        flashlight = GameObject.FindGameObjectWithTag("flashlight");
        text = bubble_box.GetComponentInChildren<Text>();
        bubble_box.SetActive(false);
        character.SetActive(false);
        mobile.SetActive(false);
        rope.SetActive(false);
        flashlight.SetActive(false);
        back_img.SetActive(false);
        brown_img.SetActive(false);
        red_img.SetActive(false);
        black_img.SetActive(false);
        text.enabled = false;
        pickup = true;
        backpack = false;
        red = false;
        brown = false;
        black = false;
        win = false;
        StartCoroutine(ShowMessage("Trace the Code", 3));
    }

    // Update is called once per frame
    void Update()
    {
        if (pickup && !win)
        {   //Backpack 
            if (CheckCloseToTag("backpack", distance))
            {
                g = GameObject.FindGameObjectWithTag("backpack");
                backpack = true;
                collect.Play();
                g.SetActive(false);
                back_img.SetActive(true);
                counter--;
            }
            else if ((CheckCloseToTag("brown", distance) || CheckCloseToTag("red", distance) || CheckCloseToTag("black", distance))
               && !backpack && !red && !brown && !black)
            {
                StartCoroutine(ShowMessage("You have to collect the Backpack first", 4));
            }
            //Red
            if (CheckCloseToTag("red", distance) && backpack && !red)
            {
                top = GameObject.FindGameObjectWithTag("reed");
                chestAnim = top.GetComponent<ChestAnimition>();
                chestAnim.start = true;
                turning.stop = true;
                Debug.Log(turning.stop);
                Debug.Log(chestAnim.start);
                if (chestAnim.finish)
                {
                    red = true;
                    collect.Play();
                    StartCoroutine(ShowHideObjects(flashlight));
                    red_img.SetActive(true);
                    counter--;
                }
            }
            else if ((CheckCloseToTag("brown", distance) || CheckCloseToTag("black", distance))
                && backpack && !red && !brown && !black)
            {
                StartCoroutine(ShowMessage("You have to check Red box first", 2));
            }
            //Black
            if (CheckCloseToTag("black", distance) && backpack && red && !black)
            {
                top = GameObject.FindGameObjectWithTag("black_top");
                chestAnim = top.GetComponent<ChestAnimition>();
                chestAnim.start = true;
                turning.stop = true;
                if (chestAnim.finish)
                {
                    black = true;
                    collect.Play();
                    StartCoroutine(ShowHideObjects(mobile));
                    black_img.SetActive(true);
                    counter--;
                }
            }
            else if ((CheckCloseToTag("brown", distance))
               && backpack && red && !brown && !black)
            {
                StartCoroutine(ShowMessage("You have to check Black box first", 4));
            }
            //Brown
            if (CheckCloseToTag("brown", distance) && backpack && black && red && !brown)
            {
                top = GameObject.FindGameObjectWithTag("brown_top");
                chestAnim = top.GetComponent<ChestAnimition>();
                chestAnim.start = true;
                turning.stop = true;
                if (chestAnim.finish)
                {
                    collect.Play();
                    StartCoroutine(ShowHideObjects(rope));
                    brown_img.SetActive(true);
                    counter--;
                }
            }
        }

        if (counter == 0 && !win)
        {
            win = true;
            turning.stop = true;
            StartCoroutine(PlayWinning());
            StartCoroutine(ShowMessage("Congratulations you completed Level 4", 3));
        }
        if (win)
        {
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

    IEnumerator ShowHideObjects(GameObject g)
    {
        g.SetActive(true);
        yield return new WaitForSecondsRealtime(5);
        g.SetActive(false);
        turning.stop = false;
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(4);
        #pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(name: "If_Video");
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
