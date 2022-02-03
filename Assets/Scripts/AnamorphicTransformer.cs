using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnamorphicTransformer : MonoBehaviour
{
    [SerializeField] GameObject[] markerArray;
    [SerializeField] GameObject[] sliceArray;
    [SerializeField] GameObject solutionPoint;
    string newMode;
    //int tracker;

    // repositioner/scaler
    [SerializeField] public GameObject model;
    private float markerdist;
    private float scale;
    private Vector3 centerPosition;
    List<Vector3> targetPosition = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.Anamorphosis_Events.ON_MARKER_MODE_CHANGE, this.OnMarkerModeChange);

        newMode = ParamConstants.Tracking_Modes.ZERO_MARKER_MODE;
    }

    private void Anamorphosize()
    {
        float cx = solutionPoint.transform.position.x; //Camera.main.transform.position.x;
        float cy = solutionPoint.transform.position.y; //Camera.main.transform.position.y;
        float cz = solutionPoint.transform.position.z; //Camera.main.transform.position.z;

        foreach (GameObject origslice in sliceArray)
        {
            /**
            * Randomizing scale
            */
            Debug.Log("Original" + origslice.transform.position);
            GameObject slice = GameObject.Instantiate(origslice, origslice.transform.position, origslice.transform.rotation);
            Debug.Log("Copy" + slice.transform.position);
            slice.SetActive(true);

            //randomly scale
            float s = UnityEngine.Random.value + 0.5f;
            slice.transform.localScale *= s;
            //Debug.Log(s);

            //reposition x
            float mx = slice.transform.position.x;
            float nx = cx - ((cx - mx) * s);

            //reposition y
            float my = slice.transform.position.y;
            float ny = cy - ((cy - my) * s);

            //reposition z
            float mz = slice.transform.position.z;
            float nz = cz - ((cz - mz) * s);

            slice.transform.position = new Vector3(nx, ny, nz);
            Debug.Log("After" + slice.transform.position);

            /**
            * Change parent based on scale
            */

            //Debug.Log(newMode);
            // for one marker, parent everything to that marker
            if(newMode == ParamConstants.Tracking_Modes.ONE_MARKER_MODE)
            {
                //Debug.Log("PLACED IN ONE");
                slice.transform.parent = markerArray[0].transform;
            }
            // for two markers, parent larger slices to farther marker, smmaller to closer marker
            else if(newMode == ParamConstants.Tracking_Modes.TWO_MARKER_MODE)
            {
                //Debug.Log("PLACED IN TWO");
                if(s > 1.0f) //check if father/closer to the solution point
                    slice.transform.parent = markerArray[0].transform;
                else
                    slice.transform.parent = markerArray[1].transform;
            }
            // for four markers, determine if smaller or larger
            // then parent to left marker if closer to left, accordingly for right marker
            else if(newMode == ParamConstants.Tracking_Modes.FOUR_MARKER_MODE)
            {
                //Debug.Log("PLACED IN THREE");
                if(s > 1.0f) //check if father/closer to the solution point
                {
                    float xmid = (markerArray[0].transform.position.x + markerArray[1].transform.position.x)/2;
                    if(slice.transform.position.x < xmid)
                        slice.transform.parent = markerArray[0].transform;
                    else
                        slice.transform.parent = markerArray[1].transform;
                }
                else
                {
                    float xmid = (markerArray[2].transform.position.x + markerArray[3].transform.position.x)/2;
                    if(slice.transform.position.x < xmid)
                        slice.transform.parent = markerArray[2].transform;
                    else
                        slice.transform.parent = markerArray[3].transform;
                }
            }
            else
            {
                //Debug.Log("PLACED IN ZERO");
                Destroy(slice);
            }
        }
        //Debug.Log("ANAMORPHOSIZED");
    }

    private void OnMarkerModeChange(Parameters parameters)
    {
        // delete all marker children
        foreach (GameObject marker in markerArray)
        {
            foreach (Transform child in marker.transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("DELETED");
        }

        // get new marker mode
        newMode = parameters.GetStringExtra(ParamConstants.Extra_Keys.MARKER_MODE, ParamConstants.Tracking_Modes.ZERO_MARKER_MODE);
        Debug.Log(newMode);

        // reposition model holder
        Reposition();

        // anamorphosize and redistribute slices
        if(newMode != ParamConstants.Tracking_Modes.ZERO_MARKER_MODE)
            Anamorphosize();
    }

    private void Reposition()
    {
        model.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        model.transform.position = new Vector3(0.0f, 0.0f, 0.0f);

        if(newMode != ParamConstants.Tracking_Modes.ZERO_MARKER_MODE)
        {
            this.targetPosition.Add(markerArray[0].transform.position);

            if(newMode == ParamConstants.Tracking_Modes.TWO_MARKER_MODE)
                this.targetPosition.Add(markerArray[1].transform.position);
            
            if(newMode == ParamConstants.Tracking_Modes.FOUR_MARKER_MODE)
            {
                this.targetPosition.Add(markerArray[2].transform.position);
                this.targetPosition.Add(markerArray[3].transform.position);
            }
            
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

            Debug.Log(centerPosition);

            this.targetPosition.Clear();
        }
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

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Anamorphosis_Events.ON_MARKER_MODE_CHANGE);
    }
}