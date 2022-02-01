using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnamorphicTransformer : MonoBehaviour
{
    [SerializeField] GameObject[] markerArray;
    [SerializeField] GameObject[] sliceArray;
    [SerializeField] Vector3 solutionPoint;
    string newMode;
    //int tracker;

    // Start is called before the first frame update
    void Start()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.Anamorphosis_Events.ON_MARKER_MODE_CHANGE, this.OnMarkerModeChange);

        newMode = ParamConstants.Tracking_Modes.ZERO_MARKER_MODE;
    }

    private void Anamorphosize()
    {
        foreach (GameObject origslice in sliceArray)
        {
            /**
            * Randomizing scale
            */
            GameObject slice = GameObject.Instantiate(origslice);
            slice.SetActive(true);

            //randomly scale
            float s = Random.value + 0.5f;
            slice.transform.localScale *= s;
            Debug.Log(s);

            //reposition x
            float cx = solutionPoint.x; //Camera.main.transform.position.x;
            float mx = slice.transform.position.x;
            float nx = cx - ((cx - mx) * s);

            //reposition y
            float cy = solutionPoint.y; //Camera.main.transform.position.y;
            float my = slice.transform.position.y;
            float ny = cy - ((cy - my) * s);

            //reposition z
            float cz = solutionPoint.z; //Camera.main.transform.position.z;
            float mz = slice.transform.position.z;
            float nz = cz - ((cz - mz) * s);

            slice.transform.position = new Vector3(nx, ny, nz);

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
        foreach (GameObject marker in markerArray)
        {
            foreach (Transform child in marker.transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("DELETED");
        }

        newMode = parameters.GetStringExtra(ParamConstants.Extra_Keys.MARKER_MODE, ParamConstants.Tracking_Modes.ZERO_MARKER_MODE);
        Debug.Log(newMode);

        if(newMode != ParamConstants.Tracking_Modes.ZERO_MARKER_MODE)
            Anamorphosize();
    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Anamorphosis_Events.ON_MARKER_MODE_CHANGE);
    }
}