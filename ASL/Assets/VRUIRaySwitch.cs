using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUIRaySwitch : MonoBehaviour
{
    public Vector3 lobbyPos = new Vector3(0,0,0);
    public GameObject UIRayInteractor;

    private bool checkDistance()
    {
        
        Vector3 distance = transform.position - lobbyPos;
        Debug.Log("MAG:" + distance.magnitude);
        if (distance.magnitude < 15f) return true; //is close enough
        return false;
    }

    private void Update()
    {
      if(checkDistance())
        {
            enableRay();
            return;
        } else
        {
            disableRay();
            return;
        }
    }

    private void enableRay()
    {
        UIRayInteractor.SetActive(true);
    }

    private void disableRay()
    {
        UIRayInteractor.SetActive(false);
    }
}
