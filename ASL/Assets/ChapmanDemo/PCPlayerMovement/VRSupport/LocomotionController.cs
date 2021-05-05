using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour {

    public XRController TeleportRay;
    public GameObject TeleportReticle;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;
    // Update is called once per frame
    void Update()
    {
        TeleportRay.gameObject.SetActive(CheckIfActivated(TeleportRay));
        TeleportReticle.SetActive(CheckIfActivated(TeleportRay));
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
