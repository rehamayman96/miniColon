using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    private GameObject g;
    private GameObject img;
    private GameObject bubble_box;
    private Text text;
    private AudioSource[] sounds;
    private AudioSource collect;
    private AudioSource winning;
    private float distance;
    private Turning turning;
    //from kinect handclose 
    public bool pickup;

    //objects flags
    public bool win;
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
        text = bubble_box.GetComponentInChildren<Text>();
        pickup = true;
        win = false;
        

    }

    // Update is called once per frame
    void Update()
    {
        switch (counter)
        {
            case 4: ShowMessage("Try to lean Left"); break;
            case 3: ShowMessage("Now try to lean Right"); break;
            case 2: ShowMessage("Pick up the basket by getting near to it an close your Right hand"); break;
            case 1: ShowMessage("Now Wave to go to next Level"); break;
        }
        if (turning.turnLeft && counter == 4)
        {
            counter--;
        }
        else if (turning.turnRight && counter == 3)
        {
            counter--;
        }
        if (pickup && !win && counter == 2)
        {   //Basket 
            if (CheckCloseToTag("basket", distance))
            {
                g = GameObject.FindGameObjectWithTag("basket");
                img = GameObject.FindGameObjectWithTag("basket_img");
                collect.Play();
                g.SetActive(false);
                img.SetActive(false);
                counter--;
            }
        }

        if (counter == 1 && !win)
        {
            win = true;
            turning.move = false;
            StartCoroutine(PlayWinning());
        }
        if (win)
        {
            //StartCoroutine(NextLevel());
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

    void ShowMessage(string message)
    {
        text.text = message;
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(4);
        #pragma warning disable CS0618 // Type or member is obsolete
        Application.LoadLevel(name: "Level_1");
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

