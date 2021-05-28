using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraTracker : MonoBehaviour
{
    public static Camera MainCamera;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Camera>() != null)
        {
            MainCamera = GetComponent<Camera>();
        }
    }
}
