using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class VRPresence : MonoBehaviour
{

    public GameObject PCPlayer;
    public List<GameObject> VRPlayer;
    public List<GameObject> VRHands;
    public bool VRorPC = false;
    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);

        foreach(var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            Debug.Log("Entering VR mode ");
            Destroy(PCPlayer);
            foreach (GameObject item in VRHands)
            {
                item.SetActive(true);
            }
        } else
        {
            Debug.Log("Entering PC mode ");
            foreach (var item in VRPlayer)
            {
                Destroy(item);
            }
        }
        VRorPC = true;
    }
}
