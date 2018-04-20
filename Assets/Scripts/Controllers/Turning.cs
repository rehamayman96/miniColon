using UnityEngine;

public class Turning : MonoBehaviour {
    public bool turnLeft;
    public bool turnRight;
    public float rotatespeed = 2f;
    private float forwardspeed = 2.3f;
    public bool move;
    public bool stop;
    // Use this for initialization
    void Start () {
        move = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (move)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * forwardspeed);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (turnRight)
            {
                turnRight = false;
                turnLeft = true;
            }
            else
            {
                turnLeft = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (turnLeft)
            {
                turnLeft = false;
                turnRight = true;
            }
            else
            {
                turnRight = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            turnRight = false;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            turnLeft = false;
        }
    }

    void FixedUpdate()
    {
        if (turnLeft)
        {
            move = false;
            transform.Rotate(new Vector3(0, -20, 0) * Time.deltaTime);
            //transform.Rotate(Vector3.right * Time.deltaTime * rotatespeed);
        }else if (turnRight)
        {
            move = false;
            transform.Rotate(new Vector3(0, 20, 0) * Time.deltaTime);
        }
        else if(stop)
        {
            move = false;
        }
        else
        {
            move = true;
        }
    }
}
