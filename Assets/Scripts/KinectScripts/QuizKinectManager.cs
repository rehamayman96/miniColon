using UnityEngine;
using Windows.Kinect;

public class QuizKinectManager : MonoBehaviour
{
    public GameObject Controller;
    private Sequence_Quiz quizScript;

    // Kinect 
    private KinectSensor kinectSensor;

    // color frame and data 
    private ColorFrameReader colorFrameReader;
    private BodyFrameReader bodyFrameReader;
    private int bodyCount;
    private Body[] bodies;

    // Use this for initialization
    void Start()
    {
        quizScript = Controller.GetComponent<Sequence_Quiz>();
 
        // get the sensor object

        this.kinectSensor = KinectSensor.GetDefault();

        if (this.kinectSensor != null)
        {
            this.bodyCount = this.kinectSensor.BodyFrameSource.BodyCount;

            // color reader
            this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();

            // body data
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // body frame to use
            this.bodies = new Body[this.bodyCount];

            // start getting data from runtime
            this.kinectSensor.Open();
        }
    }

    // Update is called once per frame
    void Update()
    {

        // process bodies
        bool newBodyData = false;
        using (BodyFrame bodyFrame = this.bodyFrameReader.AcquireLatestFrame())
        {
            if (bodyFrame != null)
            {
                bodyFrame.GetAndRefreshBodyData(this.bodies);
                newBodyData = true;
            }
        }

        if (newBodyData)
        {
            // update gesture detectors with the correct tracking id
            for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
            {
                var body = this.bodies[bodyIndex];
                if (body != null)
                {
                    if (body.HandRightState == HandState.Closed && body.HandRightConfidence == TrackingConfidence.High)
                    {
                        quizScript.choice = 1; ;
                    }
                    else if (body.HandRightState == HandState.Open && body.HandRightConfidence == TrackingConfidence.High)
                    {
                        quizScript.choice = 2; ;
                    }
                    else if (body.HandRightState == HandState.Lasso && body.HandRightConfidence == TrackingConfidence.High)
                    {
                        quizScript.choice = 3;
                    }

                }
            }
        }
    }
}