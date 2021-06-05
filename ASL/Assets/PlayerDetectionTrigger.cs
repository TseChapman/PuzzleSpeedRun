using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetectionTrigger : MonoBehaviour
{
    public EventSync myEventSync;
    public UnityEvent OnPlayerEntered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void winMessagePopup()
    {
        Debug.Log("WIN POP UP");
        GameObject[] UICanvas = GameObject.FindGameObjectsWithTag("UI");
        foreach (GameObject canvas in UICanvas)
        {
            Transform winMessage = canvas.transform.Find("winMessage");
            if (winMessage != null)
            {
                winMessage.gameObject.SetActive(true);
            }
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        myEventSync.Activate("DisplayWinMessage");
        //active event 
    }

}
