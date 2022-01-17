using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnamorphicTransformer : MonoBehaviour
{
    [SerializeField] GameObject[] sliceArray;
    [SerializeField] Vector3 solutionPoint;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject slice in sliceArray)
        {
            Anamorphosize(slice);
        }
    }

    private void Anamorphosize(GameObject slice)
    {
        //randomly scale
        float s = Random.value + 0.5f;
        slice.transform.localScale *= s;

        //reposition x
        float cx = Camera.main.transform.position.x;
        float mx = slice.transform.position.x;
        float nx = cx - ((cx - mx) * s);

        //reposition y
        float cy = Camera.main.transform.position.y;
        float my = slice.transform.position.y;
        float ny = cy - ((cy - my) * s);

        //reposition z
        float cz = Camera.main.transform.position.z;
        float mz = slice.transform.position.z;
        float nz = cz - ((cz - mz) * s);

        slice.transform.position = new Vector3(nx, ny, nz);
    }
}
