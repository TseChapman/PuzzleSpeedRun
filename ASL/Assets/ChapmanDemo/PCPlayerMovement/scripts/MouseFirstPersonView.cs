using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseFirstPersonView : MonoBehaviour
{
    public float mouseSensitivity = 1f; //This is sensitivity for mouse 
    public Slider mouseSens;
    public Transform Rig; //This store parent (player object)
    float xRotation = 0f;   //This store calculated X Rotation by mouse
    public float turnSmoothTime = 0.1f;
    public float speed = 1f;
    public Transform cam;
    public CharacterController controller;
    float turnSmoothVelocity;
    // Start is called before the first frame update
    void Start()
    {
        Rig = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {    
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime / 4;
            //xRotation -= mouseY;
            //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            //if (Rig)
            //    Rig.Rotate(Vector3.up * mouseX);

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if(direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.fixedDeltaTime);
            }

        }
        
    }

    public void updateMouseSens(Slider changer)
    {
        mouseSens = changer;
        float input = (float)mouseSens.value;
        mouseSensitivity = input;
    }
}
