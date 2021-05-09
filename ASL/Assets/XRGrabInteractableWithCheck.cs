using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableWithCheck : XRGrabInteractable
{
    private isPicked isPickedScript;
    // Start is called before the first frame update
    void Start()
    {
        isPickedScript = transform.GetChild(0).GetComponent<isPicked>();
        if (isPickedScript)
            Debug.Log("is picked Script found.");
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        if (isPickedScript.isPickedUp())
        {
            base.OnSelectEntered(interactor);
            isPickedScript.pickUp();
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        isPickedScript.release();
    }
}
