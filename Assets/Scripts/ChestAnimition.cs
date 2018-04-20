using UnityEngine;

public class ChestAnimition: MonoBehaviour {
    float x;
    public bool start,finish;
	// Use this for initialization
	void Start () {
        x = transform.eulerAngles.x;
        finish = false;
        start = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (x < 420 && start)
        {
            x += Time.deltaTime * 18;
            transform.rotation = Quaternion.Euler(-x, 0, 0);
        }
        else if(x > 420)
        {
            finish = true;
        }
    }
}
