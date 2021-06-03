using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUIRaySwitch : MonoBehaviour
{
    GameObject[] startButton;
    public GameObject UIRayInteractor;
    private void Start()
    {
            startButton = GameObject.FindGameObjectsWithTag("LobbyStartButton");
    }

    private bool checkDistance()
    {
        if (startButton == null)
        {
            Debug.Log("start button not found");
            return false;
        }
        Vector3 distance = transform.position - startButton[0].transform.position;
        if (distance.magnitude < 15f) return true; //is close enough
        return false;
    }

    private void Update()
    {
      if(checkDistance() && UIRayInteractor.active == false)
        {
            enableRay();
            return;
        }
      if (checkDistance() && UIRayInteractor.active == true)
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
