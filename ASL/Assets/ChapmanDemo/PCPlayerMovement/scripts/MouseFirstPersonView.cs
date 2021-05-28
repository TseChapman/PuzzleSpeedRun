using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFirstPersonView : MonoBehaviour
{
    public float mouseSensitivity = 1f; //This is sensitivity for mouse 
    public Transform Rig; //This store parent (player object)
    float xRotation = 0f;   //This store calculated X Rotation by mouse
    // Start is called before the first frame update
    void Start()
    {
        Rig = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {    
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (Rig)
        Rig.Rotate(Vector3.up * mouseX);
    }
}
