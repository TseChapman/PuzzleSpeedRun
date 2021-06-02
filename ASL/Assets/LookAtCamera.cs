using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion q = Quaternion.LookRotation(Camera.main.transform.forward);
        q = transform.rotation * q;
        q = Quaternion.Inverse(transform.rotation) * q;
        q.eulerAngles = new Vector3(0, 0, q.eulerAngles.z);
        transform.rotation = q;
    }
}
