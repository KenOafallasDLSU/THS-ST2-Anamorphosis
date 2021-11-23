using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System;

public class ScaleModel : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] public GameObject model;
    [SerializeField] public GameObject target1;
    [SerializeField] public GameObject target2;
    private float markerdist;
    private float scale;
    private Vector3 center;

    [SerializeField] private float distanceError;
    [SerializeField] private float angleError;

    [SerializeField] private float correctDist;
    [SerializeField] private float correctXAngle;
    [SerializeField] private float correctYAngle;
    [SerializeField] private float correctDelta;

    [SerializeField] private GameObject plane;

    void Start() {
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> targets = new List<Vector3>();
        targets.Add(this.target1.transform.position);
        targets.Add(this.target1.transform.position);
        center = getCenter(targets);

        this.markerdist = Mathf.Abs(target1.transform.position.z - target2.transform.position.z);
        this.scale = 1f + (float)System.Math.Round(markerdist, 1);
        this.transform.position = this.transform.position * scale;
        model.transform.localScale = new Vector3(scale, scale, scale);
        model.transform.position = (target1.transform.position + target2.transform.position) / 2f;

        plane.transform.position = (target1.transform.position + target2.transform.position) / 2f;
        plane.transform.localRotation = target1.transform.localRotation;

        float dist = Vector3.Distance(camera.transform.position, center);
        float delta = Vector3.Angle(camera.transform.forward, center - camera.transform.position);
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
    }

    public static Vector3 getCenter(List<Vector3> v)
    {
        Vector3 temp = Vector3.zero;
        foreach (Vector3 vec in v){
            temp += vec;
        }
        return temp / v.Count;
    } 
}