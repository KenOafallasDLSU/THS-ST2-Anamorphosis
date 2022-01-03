using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System;

public class ScaledTracker : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] public GameObject model;
    [SerializeField] public GameObject target1;
    [SerializeField] public GameObject target2;
    [SerializeField] public GameObject target3;
    [SerializeField] public GameObject target4;
    private float markerdist;
    private float scale;
    private Vector3 centerPosition;

    [SerializeField] private float distanceError;
    [SerializeField] private float angleError;

    [SerializeField] private float correctDist;
    [SerializeField] private float correctXAngle;
    [SerializeField] private float correctYAngle;
    [SerializeField] private float correctDelta;

    // pseudo-target
    [SerializeField] private GameObject plane;

    // the list of Vector3 positions for every marker
    List<Vector3> targetPosition = new List<Vector3>();

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // manually add markers to the list
        this.targetPosition.Add(target1.transform.position);
        this.targetPosition.Add(target2.transform.position);
        this.targetPosition.Add(target3.transform.position);
        this.targetPosition.Add(target4.transform.position);

        // get the center of all markers in the list
        centerPosition = getCenterPosition(this.targetPosition);

        // contains the maximum x and y distances among the marker/s
        Vector3 max = getMaxVector(this.targetPosition);
        this.markerdist = max.z;

        // scale starts at 1, and scales linearly depending on the distance of the two markers
        this.scale = 1f + (float)System.Math.Round(markerdist, 1);

        if (this.scale > 1.5) // refers to the length of 2 markers
            this.model.SetActive(false); // deactivates the model
        else if (this.model.activeInHierarchy == false)
            this.model.SetActive(true); // activates the model

        // the positions also needs to scale to preserve anamorphism
        // the rotation follows the main target's rotation
        model.transform.localScale = new Vector3(scale, scale, scale);
        model.transform.position = centerPosition;
        plane.transform.position = centerPosition;
        plane.transform.localRotation = target1.transform.localRotation;

        float dist = Vector3.Distance(camera.transform.position, centerPosition);
        float delta = Vector3.Angle(camera.transform.forward, centerPosition - camera.transform.position);
        float x = plane.transform.localRotation.eulerAngles.x;
        float y = plane.transform.localRotation.eulerAngles.y;

        bool goodDist = dist > correctDist * scale - distanceError && dist < correctDist * scale + distanceError;
        bool goodAngle = delta > correctDelta * scale - angleError && delta < correctDelta * scale + angleError;
        bool goodX = x > correctXAngle - angleError && x < correctXAngle + angleError;
        bool goodY = y > correctYAngle - angleError && y < correctYAngle + angleError;

        if (goodDist && goodAngle && goodX && goodY)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.Anamorphosis_Events.ON_WIN);
        }

        this.targetPosition.Clear();
    }


    // returns the Vector3 center position of the markers in the list
    public static Vector3 getCenterPosition(List<Vector3> v)
    {
        Vector3 temp = new Vector3();
        foreach (Vector3 vec in v)
            temp += vec;
        return temp / v.Count;
    }

    // returns a vector with maximum x and y distances
    public static Vector3 getMaxVector(List<Vector3> v)
    {
        float x = 0;
        float z = 0;
        foreach (Vector3 a in v)
            foreach (Vector3 b in v)
            {
                x = Math.Abs(a.x - b.x) > x ? Math.Abs(a.x - b.x) : x;
                z = Math.Abs(a.z - b.z) > z ? Math.Abs(a.z - b.z) : z;
            }
        return new Vector3(x, 0, z);
    }
}