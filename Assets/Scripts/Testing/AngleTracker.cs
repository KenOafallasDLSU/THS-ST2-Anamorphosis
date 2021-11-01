using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class AngleTracker : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject target;
    [SerializeField] private Text camText;
    [SerializeField] private Text targetText;
    [SerializeField] private Text distanceText;
    [SerializeField] private Text camAngText;
    [SerializeField] private Text tarAngText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(camera.transform.position, target.transform.position);
        
        camText.text = "Cam Pos: " + camera.transform.position.ToString();
        targetText.text = "Tar Pos: " + target.transform.position.ToString();
        distanceText.text = "DIST: " + dist.ToString();
        camAngText.text = "Cam Ang: " + camera.transform.localRotation.eulerAngles.ToString();
        tarAngText.text = "Tar Ang: " + target.transform.localRotation.eulerAngles.ToString();
    }
}
