using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRPushInteractable : XRGrabInteractable
{
    private Vector3 initialAttachLocalPos;
    private Quaternion initialAttachLocalRot;
    private InteractableASLObject interactableASLObjectScript;
    private bool IpickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        interactableASLObjectScript = GetComponent<InteractableASLObject>();
        if (!attachTransform)
        {
            GameObject grab = new GameObject("Grab Pivot");
            grab.transform.SetParent(transform, false);
            attachTransform = grab.transform;
        }
        initialAttachLocalPos = attachTransform.localPosition;
        initialAttachLocalRot = attachTransform.localRotation;

    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        
        if (!interactableASLObjectScript.isInteracting)
        {
            interactableASLObjectScript.startInteractingWithObject();
            if (interactor is XRDirectInteractor)
            {
                attachTransform.position = interactor.transform.position;
                attachTransform.rotation = interactor.transform.rotation;
            }
            else
            {
                attachTransform.localPosition = initialAttachLocalPos;
                attachTransform.localRotation = initialAttachLocalRot;
            }
            base.OnSelectEntered(interactor);           
        }
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        if (!interactableASLObjectScript.isInteracting)
        {
            interactableASLObjectScript.startInteractingWithObject();
            GetComponent<PushableObjectFriction>().lockY();
            base.OnSelectEntering(interactor);          
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        GetComponent<PushableObjectFriction>().unlockY();
        interactableASLObjectScript.stopInteractingWithObject();
    }
}
