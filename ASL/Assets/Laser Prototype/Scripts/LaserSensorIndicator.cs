using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSensorIndicator : MonoBehaviour
{

    public int Threshold;
    public Color On;
    public Color Off;
    private int activations;

    // Start is called before the first frame update
    void Start()
    {
        activations = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LaserHitStart()
    {
        activations++;
        if (activations >= Threshold)
        {
            if (GetComponent<MeshRenderer>() != null)
            {
                GetComponent<MeshRenderer>().material.color = On;
            }
            if (GetComponent<Light>() != null)
            {
                GetComponent<Light>().color = On;
            }
        }
    }
    public void LaserHitEnd()
    {
        activations--;
        if (activations < Threshold)
        {
            if (GetComponent<MeshRenderer>() != null)
            {
                GetComponent<MeshRenderer>().material.color = Off;
            }
            if (GetComponent<Light>() != null)
            {
                GetComponent<Light>().color = Off;
            }
        }
    }
}
