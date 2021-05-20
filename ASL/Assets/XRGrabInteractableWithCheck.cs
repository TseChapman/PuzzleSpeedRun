using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableWithCheck : XRGrabInteractable
{
    private InteractableASLObject interactableASLObjectScript;
    private bool isHandlingLocally = false;
    // Start is called before the first frame update
    void Start()
    {
        interactableASLObjectScript = GetComponent<InteractableASLObject>();
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        if (!interactableASLObjectScript.isInteracting)
        {
            base.OnSelectEntering(interactor);
            interactableASLObjectScript.startInteractingWithObject();
        }
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        if (!interactableASLObjectScript.isInteracting)
        {
            base.OnSelectEntered(interactor);
            interactableASLObjectScript.startInteractingWithObject();
            isHandlingLocally = true;
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        if (isHandlingLocally)
        {
            Debug.Log("isPickedScript.OnSelectedExiting");
            base.OnSelectExiting(interactor);
            interactableASLObjectScript.stopInteractingWithObject();
            isHandlingLocally = false;
        }   
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        
        Debug.Log("isPickedScript.OnSelectedExited");
            base.OnSelectExited(interactor);
            interactableASLObjectScript.stopInteractingWithObject();
        if (isHandlingLocally)
        {
            isHandlingLocally = false;
        }
    }
}
