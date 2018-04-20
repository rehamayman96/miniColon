using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PickUp3 : MonoBehaviour
{
    private GameObject g;
    private GameObject img;
    private GameObject open_img;
    private GameObject mobile;
    private GameObject rope;
    private GameObject flashlight;
    private GameObject bubble_box;
    private GameObject character;
    private GameObject code_snippet;
    private GameObject top;
    private GameObject brown_img;
    private GameObject red_img;
    private GameObject black_img;
    private Text code_snippetText;
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
    //bool backpack;
    bool red;
    bool brown;
    bool black;
    private bool win;
    int counter = 3;
    string code;
    // Use this for initialization
    void Start()
    {
        distance = 3.5f;
        turning = GetComponentInParent<Turning>();
        sounds = GetComponents<AudioSource>();
        collect = sounds[0];
        winning = sounds[1];
        bubble_box = GameObject.FindGameObjectWithTag("bubble_box");
        character = GameObject.FindGameObjectWithTag("character");
        code_snippet = GameObject.FindGameObjectWithTag("code_snippet");
        mobile = GameObject.FindGameObjectWithTag("mobile");
        rope = GameObject.FindGameObjectWithTag("rope");
        flashlight = GameObject.FindGameObjectWithTag("flashlight");
        brown_img = GameObject.FindGameObjectWithTag("broown");
        red_img = GameObject.FindGameObjectWithTag("rod");
        black_img = GameObject.FindGameObjectWithTag("blackberry_img");
        text = bubble_box.GetComponentInChildren<Text>();
        code_snippetText = code_snippet.GetComponentInChildren<Text>();
        bubble_box.SetActive(false);
        character.SetActive(false);
        code_snippet.SetActive(false);
        mobile.SetActive(false);
        rope.SetActive(false);
        flashlight.SetActive(false);
        brown_img.SetActive(false);
        red_img.SetActive(false);
        black_img.SetActive(false);
        text.enabled = false;
        pickup = true;
        //backpack = false;
        red = false;
        brown = false;
        black = false;
        win = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pickup && !win)
        {   //Backpack 
        //    if (CheckCloseToTag("backpack", distance) && !backpack)
        //    {
        //        g = GameObject.FindGameObjectWithTag("backpack");
        //        img = GameObject.FindGameObjectWithTag("basket_img");
        //        backpack = true;
        //        collect.Play();
        //        g.SetActive(false);
        //        img.SetActive(false);
        //        counter--;
        //    }
            //Brown
            if (CheckCloseToTag("brown", distance) && !brown)
            {
                img = GameObject.FindGameObjectWithTag("brown_img");
                top = GameObject.FindGameObjectWithTag("brown_top");
                chestAnim = top.GetComponent<ChestAnimition>();
                chestAnim.start = true;
                turning.stop = true;
                if (chestAnim.finish)
                {
                    brown = true;
                    collect.Play();
                    img.SetActive(false);
                    brown_img.SetActive(true);
                    code = "if Brown_Box :" + '\n' + "    collect_Rope()";
                    StartCoroutine(ShowHideObjects(rope));
                    StartCoroutine(ShowCode(code));
                    counter--;
                }
            }
            //Red
            if (CheckCloseToTag("red", distance) && !red)
            {
                img = GameObject.FindGameObjectWithTag("red_img");
                top = GameObject.FindGameObjectWithTag("red_top");
                chestAnim = top.GetComponent<ChestAnimition>();
                chestAnim.start = true;
                turning.stop = true;
                if (chestAnim.finish)
                {
                    red = true;
                    collect.Play();
                    img.SetActive(false);
                    red_img.SetActive(true);
                    code = "if Red_Box :" + '\n' + "    collect_Flashlight()";
                    StartCoroutine(ShowHideObjects(flashlight));
                    StartCoroutine(ShowCode(code));
                    counter--;
                }
            }
            //Black
            if (CheckCloseToTag("black", distance) && !black)
            {
                img = GameObject.FindGameObjectWithTag("black_img");
                top = GameObject.FindGameObjectWithTag("black_top");
                chestAnim = top.GetComponent<ChestAnimition>();
                chestAnim.start = true;
                turning.stop = true;
                if (chestAnim.finish)
                {
                    black = true;
                    collect.Play();
                    img.SetActive(false);
                    black_img.SetActive(true);
                    code = "if Black_Box :" + '\n' + "    collect_Mobile()";
                    StartCoroutine(ShowHideObjects(mobile));
                    StartCoroutine(ShowCode(code));
                    counter--;
                }
            }
        }

        if (counter == 0 && !win)
        {
            win = true;
            turning.stop = true;
            StartCoroutine(PlayWinning());
            StartCoroutine(ShowMessage("Congratulations you completed Level 3", 3));
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

    IEnumerator ShowCode(string s)
    {
        code_snippet.SetActive(true);
        code_snippetText.text = s;
        yield return new WaitForSecondsRealtime(7);
        code_snippet.SetActive(false);
        turning.stop = false;
    }

    IEnumerator ShowHideObjects(GameObject g)
    {
        g.SetActive(true);
        yield return new WaitForSecondsRealtime(5);
        g.SetActive(false);
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(4);
        #pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(name: "Level_4");
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
