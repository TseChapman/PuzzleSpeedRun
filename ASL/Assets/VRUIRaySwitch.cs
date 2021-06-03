using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUIRaySwitch : MonoBehaviour
{
    void OnEnable()
    {
        GameObject VRObject = GameObject.Find("Camera Offset");
        if (VRObject == null) return;
        Debug.Log("UI Interactor found: camera offset");
        GameObject VRRay = VRObject.transform.Find("UI Interactor").gameObject;
        if (VRRay == null) return;
        Debug.Log("UI Interactor found");
        VRRay.SetActive(true);
    }

    void OnDestroy()
    {
        GameObject VRObject = GameObject.Find("Camera Offset");
        if (VRObject == null) return;
        Debug.Log("UI Interactor found: camera offset");
        GameObject VRRay = VRObject.transform.Find("UI Interactor").gameObject;
        if (VRRay == null) return;
        Debug.Log("UI Interactor found");
        VRRay.SetActive(false);
    }
}
