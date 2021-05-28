using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetFrameRate : MonoBehaviour {
    public int targetFrameRate = 30;
    public int secondStartgetFrameRate = 120;

    private void Start()
    {
        InvokeRepeating("setFrameRateOne", 0f,5f);
        
    }

    private void setFrameRateOne()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
        StartCoroutine(DelayAction(1));
    }

    private void setFrameRateTwo()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = secondStartgetFrameRate;
    }

    IEnumerator DelayAction(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        setFrameRateTwo();
        //Do the action after the delay time has finished.
    }
}