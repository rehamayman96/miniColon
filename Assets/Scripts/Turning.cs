using UnityEngine;

public class Turning : MonoBehaviour {
    public bool turnLeft;
    public bool turnRight;
    private bool rotating;
    public float rotatespeed = 2f;
    public float forwardspeed = 4.0f;
    public bool move;

    // Use this for initialization
    void Start () {
        move = true;
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

            transform.Rotate(new Vector3(0, -20, 0) * Time.deltaTime);
            //transform.Rotate(Vector3.right * Time.deltaTime * rotatespeed);


        }
        if (turnRight)
        {
            transform.Rotate(new Vector3(0, 20, 0) * Time.deltaTime);
        }
    }
}
