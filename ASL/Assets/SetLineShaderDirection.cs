using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLineShaderDirection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        GetComponent<LineRenderer>().material.SetVector("_Rotation", transform.rotation * Vector3.forward);
    }
}
