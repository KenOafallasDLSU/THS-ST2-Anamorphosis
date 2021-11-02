using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class AngleTracker : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject target;

    [SerializeField] private float correctDist;
    [SerializeField] private float distanceError;

    [SerializeField] private float correctXAngle;
    [SerializeField] private float correctYAngle;
    [SerializeField] private float correctDelta;
    [SerializeField] private float angleError;

    [SerializeField] private Text camText;
    [SerializeField] private Text targetText;
    [SerializeField] private Text distanceText;
    [SerializeField] private Text camAngText;
    [SerializeField] private Text tarAngText;
    [SerializeField] private Text deltaAngText;
    [SerializeField] private Text goodText;

    // Start is called before the first frame update
    void Start()
    {
        //add checking whether actively tracking
    }

    // Update is called once per frame
    void Update()
    {
        //add condition of actively tracking

        //store values for easier referencing
        float dist = Vector3.Distance(camera.transform.position, target.transform.position);
        float delta = Vector3.Angle(camera.transform.forward, target.transform.position - camera.transform.position);
        float x = target.transform.localRotation.eulerAngles.x;
        float y = target.transform.localRotation.eulerAngles.y;

        bool goodDist = dist > correctDist - distanceError && dist < correctDist + distanceError;
        bool goodAngle = delta > correctDelta - angleError && delta < correctDelta + angleError;
        bool goodX = x > correctXAngle - angleError && x < correctXAngle + angleError;
        bool goodY = y > correctYAngle - angleError && y < correctYAngle + angleError;

        if(goodDist && goodAngle && goodX && goodY){
            EventBroadcaster.Instance.PostEvent(EventNames.Anamorphosis_Events.ON_WIN);
            goodText.text = "GOOD!!!";
        }
        else{
            goodText.text = "NOT GOOD";
        }

        //update Test UI
        camText.text = "Cam Pos: " + camera.transform.position.ToString();
        targetText.text = "Tar Pos: " + target.transform.position.ToString();
        distanceText.text = "DIST: " + dist.ToString();
        camAngText.text = "Cam Ang X: " + camera.transform.localRotation.eulerAngles.x.ToString() + " , Y: " + camera.transform.localRotation.eulerAngles.y.ToString();
        tarAngText.text = "Tar Ang X: " + target.transform.localRotation.eulerAngles.x.ToString() + " , Y: " + target.transform.localRotation.eulerAngles.y.ToString();
        deltaAngText.text = "DELTA: " + delta.ToString();
    }
}
