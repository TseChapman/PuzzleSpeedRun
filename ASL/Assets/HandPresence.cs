using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class HandPresence : MonoBehaviour {
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject handModePrefab;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice targetDevice;
    private GameObject spawnedHandModel;
    private Animator handAnimator;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (devices.Count == 0)
        {
            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
            if (devices.Count > 0)
            {
                targetDevice = devices[0];
                // Debug.Log("Hand Model prefab instantantiated");
                spawnedHandModel = Instantiate(handModePrefab, transform);
                transform.parent.transform.localScale = new Vector3(1, 1, 1);
                handAnimator = spawnedHandModel.GetComponent<Animator>();
            }
        }
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (devices.Count == 0) return;
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        } else
        {
            handAnimator.SetFloat("Trigger", 0);
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }

    }
}

    
