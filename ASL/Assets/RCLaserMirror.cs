using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using ASL;

public class RCLaserMirror : MonoBehaviour
{
    public InputActionProperty leftRotation;
    public InputActionProperty leftPosition;
    public InputActionProperty rightRotation;
    public InputActionProperty rightPosition;
    public InputActionProperty headPosition;
    public InputActionProperty headRotation;

    public bool UseCameraLook;

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

    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        baseColor = GetComponent<MeshRenderer>().material.color;
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
        GetComponent<ASLObject>().SendAndSetClaim(() => {
            selected = true;
            GetComponent<MeshRenderer>().material.color = Color.red;
            Gimbal.GetComponent<MeshRenderer>().material.color = Color.red;
        }, 0);
    }

    public void OnDeselect()
    {
        selected = false;
        GetComponent<MeshRenderer>().material.color = baseColor;
        Gimbal.GetComponent<MeshRenderer>().material.color = baseColor;
    }

    public void OnHoverEntered()
    {
        if (!selected)
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
            Gimbal.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
    public void OnHoverExited()
    {
        if (!selected)
        {
            GetComponent<MeshRenderer>().material.color = baseColor;
            Gimbal.GetComponent<MeshRenderer>().material.color = baseColor;
        }
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

        Vector3 leftRotVec = leftRot.eulerAngles;
        Vector3 rightRotVec = rightRot.eulerAngles;

        float yFromZPos = leftPos.z - rightPos.z;
        float yFromXPos = leftPos.x + rightPos.x;
        float xFromPos = leftPos.y + rightPos.y;

        float yLeft0 = (leftRot.eulerAngles.y - headRot.eulerAngles.y + 360) % 360;
        float yRight0 = (rightRot.eulerAngles.y - rightRot.eulerAngles.y + 360) % 360;

        float yLeft = yLeft0 > 180 ? yLeft0 - 360 : yLeft0;
        float yRight = yRight0 > 180 ? yRight0 - 360 : yRight0;

        float yFromYRot = yLeft + yRight; // (yLeft + yRight) / 2 - yHead;

        float xLeft = leftRot.eulerAngles.x > 180 ? leftRot.eulerAngles.x - 360 : leftRot.eulerAngles.x;
        float xRight = rightRot.eulerAngles.x > 180 ? rightRot.eulerAngles.x - 360 : rightRot.eulerAngles.x;
        float xFromXRot = (xLeft + xRight) / 2;

        if (selected && !GetComponent<ASLObject>().m_Mine)
        {
            OnDeselect();
        }

        if (selected)
        {

            if (UseCameraLook)
            {
                if (MainCameraTracker.MainCamera == null)
                {
                    Debug.Log("MainCameraTracker.MainCamera == null!");
                    return;
                }
                Ray ray = MainCameraTracker.MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                Debug.Log(MainCameraTracker.MainCamera);
                Debug.Log(ray);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, ~LayerMask.GetMask("BoundaryTrigger")))
                {
                    Quaternion oldRotation = Mirror.transform.rotation;
                    Transform nextTransform = Mirror.transform;
                    nextTransform.LookAt(hit.point, Vector3.up);
                    Quaternion newRotation = Quaternion.Slerp(oldRotation, nextTransform.rotation, 10f*Time.deltaTime);
                    Debug.Log(oldRotation + "  " + newRotation);
                    nextTransform.rotation = newRotation;
                    Vector3 v = nextTransform.rotation * new Vector3(0, 0, 1);
                    Debug.DrawRay(nextTransform.position + new Vector3(0, 2, 0), v);
                    //if (Vector3.Angle(v, new Vector3(0, -1, 0)) > 30)
                    //{
                        Mirror.transform.rotation = Quaternion.Slerp(Mirror.transform.rotation, nextTransform.rotation, 0.05f);
                        Vector3 rG = Gimbal.transform.rotation.eulerAngles;
                        Vector3 rM = Mirror.transform.rotation.eulerAngles;
                        Quaternion q = new Quaternion();
                        q.eulerAngles = new Vector3(rG.x, rM.y, rG.z);
                        Gimbal.transform.rotation = q;
                    //}
                }
            }
            else
            {
                float y = Gimbal.transform.localRotation.eulerAngles.y;
                //y += (yFromZPos - previousYFromZPos) * 100f + (yFromXPos - previousYFromXPos) * 50f;
                y += yFromYRot * 0.01f;
                Quaternion G = new Quaternion();
                G.eulerAngles = new Vector3(0, y, 0);
                Gimbal.transform.localRotation = G;

                float x = Mirror.transform.localRotation.eulerAngles.x;
                //x += (xFromPos - previousXFromPos) * 100;
                x += xFromXRot * 0.03f;
                G.eulerAngles = new Vector3(x, 0, 0);
                Mirror.transform.localRotation = G;
            }
        }

        previousYFromZPos = yFromZPos;
        previousYFromXPos = yFromXPos;
        previousXFromPos = xFromPos;
    }
}
