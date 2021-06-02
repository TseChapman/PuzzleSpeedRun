using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class LocomotionController : MonoBehaviour {

    public GameObject TeleportRay;
    public GameObject TeleportReticle;
    public XRNode rightHand;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;
    private bool upEntered;
    private InputDevice device;
    private Vector2 inputAxis;
    // Update is called once per frame
    private void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(rightHand);
        
    }

    void Update()
    {
        
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        upEntered = trackTeleportHand();
        TeleportRay.gameObject.SetActive(upEntered);
        TeleportReticle.SetActive(upEntered);
        Debug.Log("Right hand inputAxis: " + inputAxis + " is " + trackTeleportHand());
    }

    private bool trackTeleportHand()
    {
        if(inputAxis.y > .8 && ((inputAxis.x < .3) && (inputAxis.x > -.3)))
        {
            return true;
        }
        return false;
    }
}
