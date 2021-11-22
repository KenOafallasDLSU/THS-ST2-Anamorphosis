using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System;

public class ScaleModel : MonoBehaviour
{
    [SerializeField] public GameObject model;
    [SerializeField] public GameObject target1;
    [SerializeField] public GameObject target2;
    private float markerdist;
    private float scale;

    void Start()
    {
        this.markerdist = target1.transform.position.z - target2.transform.position.z;
        this.scale = 1.5f + (float)System.Math.Round(markerdist, 1); // more than one becomes jittery
        this.transform.position = this.transform.position * scale;
        model.transform.localScale = new Vector3(scale, scale, scale);
        model.transform.position = model.transform.position + new Vector3(0, 0, -(this.markerdist+0.1125f)/2f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}