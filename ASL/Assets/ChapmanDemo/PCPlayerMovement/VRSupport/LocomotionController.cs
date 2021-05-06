using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class LocomotionController : MonoBehaviour {

    public XRController TeleportRay;
    public GameObject TeleportReticle;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;
    public InputDeviceCharacteristics controllerCharacteristics;
    private List<InputDevice> devices = new List<InputDevice>();
    private bool upEntered;
    // Update is called once per frame
    void Update()
    {
        if (devices.Count == 0)
        {
            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        }
        upEntered = trackTeleportHand();
        TeleportRay.gameObject.SetActive(upEntered);
        TeleportReticle.SetActive(upEntered);
        
    }

    private bool trackTeleportHand()
    {
        if (devices.Count == 0) return false;
            if (devices[0].TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 input))
            {
                if(input.y > .8 && ((input.x < .3) && (input.x > -.3)))
                {
                    return true;
                }
            }
        return false;
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
