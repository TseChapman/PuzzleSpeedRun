using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRGrabPositionInteractable : XRGrabInteractable {
    private Vector3 initialAttachLocalPos;
    private Quaternion initialAttachLocalRot;
    private InteractableASLObject interactableASLObjectScript;
    private bool isHandlingLocally = false;
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
    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        if (!interactableASLObjectScript.isInteracting)
        {
            base.OnSelectEntering(interactor);
            //interactableASLObjectScript.startInteractingWithObject();
            isHandlingLocally = true;
        }
    }


    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {  
        if (!interactableASLObjectScript.isInteracting)
        {
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
            //interactableASLObjectScript.startInteractingWithObject();
            isHandlingLocally = true;
        }
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        if (isHandlingLocally)
        {
            Debug.Log("isPickedScript.OnSelectedExiting");
            base.OnSelectExiting(interactor);
            //interactableASLObjectScript.stopInteractingWithObject();
        }
        isHandlingLocally = false;
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
            Debug.Log("isPickedScript.OnSelectedExited");
            base.OnSelectExited(interactor);
            //interactableASLObjectScript.stopInteractingWithObject();
        if (isHandlingLocally)
        {
            isHandlingLocally = false;
        }
    }
}
