using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRPositionInteractable : XRGrabInteractable {
    private Vector3 initialAttachLocalPos;
    private Quaternion initialAttachLocalRot;
    // Start is called before the first frame update
    void Start()
    {
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

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        GetComponent<PushableObjectFriction>().lockRot();
        base.OnSelectEntering(interactor);
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        GetComponent<PushableObjectFriction>().unlock();
    }

    protected override void OnDeactivate(XRBaseInteractor interactor)
    {
        base.OnDeactivate(interactor);
    }
}
