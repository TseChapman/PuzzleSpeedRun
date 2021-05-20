using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class RCLaserMirror : MonoBehaviour
{
    public InputActionProperty leftRotation;
    public InputActionProperty leftPosition;
    public InputActionProperty rightRotation;
    public InputActionProperty rightPosition;
    public InputActionProperty headPosition;
    public InputActionProperty headRotation;

    public GameObject Gimbal;
    public GameObject Mirror;

    private Quaternion initialLR;
    private Vector3 initialMirrorRot;
    private Vector3 initialGimbalRot;

    private float previousYFromXPos;
    private float previousYFromZPos;
    private float previousXFromPos;

    private float initialLeftYRot;

    private bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnSelect() {
        /*
        Quaternion L = leftRotation.action.ReadValue<Quaternion>();
        Quaternion R = rightRotation.action.ReadValue<Quaternion>();
        Quaternion LR = Quaternion.Slerp(L, R, 0.5f);
        initialLR = LR;
        initialGimbalRot = Gimbal.transform.localRotation.eulerAngles;
        initialMirrorRot = Mirror.transform.localRotation.eulerAngles;
        */
        Quaternion leftRot = leftRotation.action.ReadValue<Quaternion>();
        initialLeftYRot = leftRot.eulerAngles.y;
        Debug.Log("Selected!");
        selected = true;
    }
    public void OnDeselect()
    {
        Debug.Log("Deselected!");
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 headPos = headPosition.action.ReadValue<Vector3>();
        Quaternion headRot = headRotation.action.ReadValue<Quaternion>();
        Vector3 leftPos = leftPosition.action.ReadValue<Vector3>();
        Vector3 rightPos = rightPosition.action.ReadValue<Vector3>();
        Quaternion leftRot = leftRotation.action.ReadValue<Quaternion>();
        Quaternion rightRot = rightRotation.action.ReadValue<Quaternion>();
        leftPos -= headPos;
        rightPos -= headPos;

        leftPos = headRot * leftPos;
        rightPos = headRot * rightPos;

        float yFromZPos = leftPos.z - rightPos.z;
        float yFromXPos = leftPos.x + rightPos.x;
        float xFromPos = leftPos.y + rightPos.y;

        float yFromYRot = (leftRot.eulerAngles.y - initialLeftYRot);// + (rightRot.eulerAngles.y - headRot.eulerAngles.y);

        float y = Gimbal.transform.localRotation.eulerAngles.y;
        y += (yFromZPos - previousYFromZPos) * 100f + (yFromXPos - previousYFromXPos) * 50f;
        Quaternion G = new Quaternion();
        G.eulerAngles = new Vector3(0, y, 0);
        Gimbal.transform.localRotation = G;

        float x = Mirror.transform.localRotation.eulerAngles.x;
        x += (xFromPos - previousXFromPos) * 100;
        G.eulerAngles = new Vector3(x, 0, 0);
        Mirror.transform.localRotation = G;

        previousYFromZPos = yFromZPos;
        previousYFromXPos = yFromXPos;
        previousXFromPos = xFromPos;

        /*
        Quaternion L = leftRotation.action.ReadValue<Quaternion>();
        Quaternion R = rightRotation.action.ReadValue<Quaternion>();
        Quaternion LR = Quaternion.Slerp(L, R, 0.5f);
        Quaternion D = LR * Quaternion.Inverse(initialLR);
        Vector3 E = D.eulerAngles;
        Quaternion G = new Quaternion();
        G.eulerAngles = initialGimbalRot + new Vector3(0, E.y, 0);
        Gimbal.transform.localRotation = G;
        Quaternion G2 = new Quaternion();
        G2.eulerAngles = initialMirrorRot + new Vector3(E.x, 0, 0);
        Mirror.transform.localRotation = G2;
        */
    }
}
