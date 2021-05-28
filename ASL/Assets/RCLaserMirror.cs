using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using ASL;

public class RCLaserMirror : MonoBehaviour {
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

    private bool readyToDeselect = false;

    public LayerMask ExcludeLayersFromCameraRay;

    public InputActionProperty joystickAxis;

    private LocomotionController lc;
    private ActionBasedSnapTurnProvider stp;
    // Start is called before the first frame update
    void Start()
    {
        baseColor = GetComponent<MeshRenderer>().material.color;
        getTeleportHelper();
    }

    public void OnSelect()
    {
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
        readyToDeselect = false;
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
        if (selected && Input.GetKeyUp("e"))
        {
            readyToDeselect = true;
        }
        if (Input.GetKeyDown("e") && selected && readyToDeselect)
        {
            OnDeselect();
        }
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

        Vector2 axisPos = joystickAxis.action.ReadValue<Vector2>();

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
                Ray ray = MainCameraTracker.MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 5));
                RaycastHit hit;
                //~LayerMask.GetMask("BoundaryTrigger") & ~LayerMask.GetMask("PlayerMesh") & ~LayerMask.GetMask("LaserMirror") & ~LayerMask.GetMask("LaserBeam") & ~LayerMask.GetMask("VRHandle")
                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, ~ExcludeLayersFromCameraRay))
                {
                    Transform nextTransform = Mirror.transform;
                    nextTransform.LookAt(hit.point, Vector3.up);
                    Mirror.transform.rotation = nextTransform.rotation;
                    Vector3 rG = Gimbal.transform.rotation.eulerAngles;
                    Vector3 rM = Mirror.transform.rotation.eulerAngles;
                    Quaternion q2 = new Quaternion();
                    q2.eulerAngles = new Vector3(rG.x, rM.y, rG.z);
                    Gimbal.transform.rotation = q2;
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

                Vector3 movementDir = -new Vector3(MainCameraTracker.MainCamera.transform.position.x - transform.position.x,
                    0, MainCameraTracker.MainCamera.transform.position.z - transform.position.z).normalized;

                Debug.Log("JOY AXIS: " + axisPos);
                transform.position += movementDir * axisPos.y * .01f + Quaternion.Euler(0, 90, 0) * movementDir * axisPos.x * .01f;
            }
        }

        previousYFromZPos = yFromZPos;
        previousYFromXPos = yFromXPos;
        previousXFromPos = xFromPos;
        setTeleportHelperActive(selected);

    }

    private void setTeleportHelperActive(bool boolean)
    {
        if (!stp || !lc) return;
        stp.enabled = !boolean;
        lc.enabled = !boolean;
    }
    private void getTeleportHelper()
    {
        GameObject a = GameObject.Find("VR Rig");
        Debug.Log("VR RO");
        stp = a.GetComponent<ActionBasedSnapTurnProvider>();
        lc = a.GetComponent<LocomotionController>();
    }
}