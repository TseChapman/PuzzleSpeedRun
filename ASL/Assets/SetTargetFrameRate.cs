using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetFrameRate : MonoBehaviour {
    public int targetFrameRate = 30;
    public int secondStartgetFrameRate = 120;

    private void Start()
    {
        InvokeRepeating("setFrameRateOne", 0f, 0.3f);
        InvokeRepeating("setFrameRateTwo", 0f, 0.5f);
    }

    private void setFrameRateOne()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }

    private void setFrameRateTwo()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = secondStartgetFrameRate;
    }
}