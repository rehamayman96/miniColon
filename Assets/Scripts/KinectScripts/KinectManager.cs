using UnityEngine;
using System;
using System.Collections.Generic;
using Windows.Kinect;

public class KinectManager : MonoBehaviour
{
    public GameObject Player;
    private Turning turnScript;
    private PickUp2 pickupScript;

    // Kinect 
    private KinectSensor kinectSensor;

    // color frame and data 
    private ColorFrameReader colorFrameReader;
    private byte[] colorData;
    private Texture2D colorTexture;

    private BodyFrameReader bodyFrameReader;
    private int bodyCount;
    private Body[] bodies;

    private string leanLeftGestureName = "Lean_Left";
    private string leanRightGestureName = "Lean_Right";
    private string runGestureName = "Run";

    // GUI output
    private UnityEngine.Color[] bodyColors;
    //private string[] bodyText;

    /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
    private List<GestureDetector> gestureDetectorList = null;
    private List<RunGestureDetector> rungestureDetectorList = null;
    // Use this for initialization
    void Start()
    {
        turnScript = Player.GetComponent<Turning>();
        pickupScript = Player.GetComponentInChildren<PickUp2>();
        // get the sensor object

        this.kinectSensor = KinectSensor.GetDefault();

        if (this.kinectSensor != null)
        {
            this.bodyCount = this.kinectSensor.BodyFrameSource.BodyCount;

            // color reader
            this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();

            // create buffer from RGBA frame description
            //var desc = this.kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);

            // body data
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // body frame to use
            this.bodies = new Body[this.bodyCount];

            // initialize the gesture detection objects for our gestures
            this.gestureDetectorList = new List<GestureDetector>();
            this.rungestureDetectorList = new List<RunGestureDetector>();
            for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
            {
                this.gestureDetectorList.Add(new GestureDetector(this.kinectSensor));
                this.rungestureDetectorList.Add(new RunGestureDetector(this.kinectSensor));
            }

            // start getting data from runtime
            this.kinectSensor.Open();
        }
        else
        {
            //kinect sensor not connected
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
                    if (body.HandRightState == HandState.Closed)
                    {
                        pickupScript.pickup = true;
                        Debug.Log("da5al true");
                    }
                    else if (body.HandRightConfidence == TrackingConfidence.Low)
                    {
                        pickupScript.pickup = false;
                        Debug.Log("da5al false");
                    }

                    var trackingId = body.TrackingId;
                    // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                    if (trackingId != this.gestureDetectorList[bodyIndex].TrackingId)
                    {
                        this.gestureDetectorList[bodyIndex].TrackingId = trackingId;

                        // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                        // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                        this.gestureDetectorList[bodyIndex].IsPaused = (trackingId == 0);
                        this.gestureDetectorList[bodyIndex].OnGestureDetected += CreateOnGestureHandler(bodyIndex);
                    }

                    if (trackingId != this.rungestureDetectorList[bodyIndex].TrackingId)
                    {
                        this.rungestureDetectorList[bodyIndex].TrackingId = trackingId;

                        // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                        // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                        this.rungestureDetectorList[bodyIndex].IsPaused = (trackingId == 0);
                        this.rungestureDetectorList[bodyIndex].OnRunGestureDetected += CreateOnRunGestureHandler(bodyIndex);
                    }

                }
            }
        }

    }

    private EventHandler<GestureEventArgs> CreateOnGestureHandler(int bodyIndex)
    {
        return (object sender, GestureEventArgs e) => OnGestureDetected(sender, e, bodyIndex);
    }

    private void OnGestureDetected(object sender, GestureEventArgs e, int bodyIndex)
    {
        if (e.GestureID == leanLeftGestureName)
        {
            if (e.DetectionConfidence > 0.55f)
            {
                turnScript.turnLeft = true;
            }
            else
            {
                turnScript.turnLeft = false;
            }
        }

        if (e.GestureID == leanRightGestureName)
        {
            if (e.DetectionConfidence > 0.55f)
            {
                turnScript.turnRight = true;
            }
            else
            {
                turnScript.turnRight = false;
            }
        }
    }

    //RunGesture 
    private EventHandler<RunGestureEventArgs> CreateOnRunGestureHandler(int bodyIndex)
    {
        return (object sender, RunGestureEventArgs e) => OnRunGestureDetected(sender, e, bodyIndex);
    }

    private void OnRunGestureDetected(object sender, RunGestureEventArgs e, int bodyIndex)
    {
        if (e.GestureID == runGestureName)
        {
            //Debug.Log(e.DetectionConfidence);

            if (e.DetectionConfidence > 0.42)
            {
                turnScript.move = true;
                //Debug.Log("Running");
            }
            else
            {
                turnScript.move = false;
               // Debug.Log("fsjrxdfc");

            }
        }
    }

    void OnApplicationQuit()
    {
        if (this.colorFrameReader != null)
        {
            this.colorFrameReader.Dispose();
            this.colorFrameReader = null;
        }

        if (this.bodyFrameReader != null)
        {
            this.bodyFrameReader.Dispose();
            this.bodyFrameReader = null;
        }

        if (this.kinectSensor != null)
        {
            if (this.kinectSensor.IsOpen)
            {
                this.kinectSensor.Close();
            }

            this.kinectSensor = null;
        }
    }

}

