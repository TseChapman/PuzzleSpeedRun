using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableWithCheck : XRGrabInteractable
{
    private isPicked isPickedScript;
    private bool IpickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        isPickedScript = transform.GetChild(0).GetComponent<isPicked>();
        if (isPickedScript)
            Debug.Log("isPickedScript.is picked Script found.");
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        Debug.Log("isPickedScript.isPickedUp(): " + isPickedScript.isPickedUp());
        if (!isPickedScript.isPickedUp())
        {
            base.OnSelectEntering(interactor);
            isPickedScript.pickUp();
            
        }
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("isPickedScript.isPickedUp(): " + isPickedScript.isPickedUp());
        if (!isPickedScript.isPickedUp())
        {
            base.OnSelectEntered(interactor);
            isPickedScript.pickUp();
            IpickedUp = true;
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        if (IpickedUp)
        {
            Debug.Log("isPickedScript.OnSelectedExiting");
            base.OnSelectExiting(interactor);
            isPickedScript.release();
        }
        
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        if (IpickedUp)
        {
            Debug.Log("isPickedScript.OnSelectedExited");
            base.OnSelectExited(interactor);
            isPickedScript.release();
            IpickedUp = false;
        }
    }
}
