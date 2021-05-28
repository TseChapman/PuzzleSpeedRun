using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVideoArea : MonoBehaviour {
    public GameObject[] toPlay;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PCPlayer" || other.gameObject.name == "VR Rig" || other.gameObject.name == "VR Rig ")
        {
            activateVideo();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PCPlayer" || other.gameObject.name == "VR Rig" || other.gameObject.name == "VR Rig ")
        {
            deactivateVideo();
        }
    }
    private void activateVideo()
    {
        foreach (GameObject item in toPlay)
        {
            item.SetActive(true);
        }
    }

    private void deactivateVideo()
    {
        foreach (GameObject item in toPlay)
        {
            item.SetActive(false);
        }
    }

}
