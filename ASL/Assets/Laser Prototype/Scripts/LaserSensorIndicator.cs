using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserSensorIndicator : MonoBehaviour
{

    public int Threshold;
    public Color On;
    public Color Off;
    private int activations;

    [Serializable]
    public struct Indicator
    {
        public GameObject obj;
        public int MaterialIndex;
    }

    public List<Indicator> indicators;

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
            foreach (Indicator i in indicators)
            {
                if (i.obj.GetComponent<MeshRenderer>() != null)
                {
                    i.obj.GetComponent<MeshRenderer>().materials[i.MaterialIndex].SetColor("_EmissionColor", On);
                }
                if (i.obj.GetComponent<Light>() != null)
                {
                    i.obj.GetComponent<Light>().color = On;
                }
            }
        }
    }
    public void LaserHitEnd()
    {
        activations--;
        if (activations < Threshold)
        {
            foreach (Indicator i in indicators)
            {
                if (i.obj.GetComponent<MeshRenderer>() != null)
                {
                    i.obj.GetComponent<MeshRenderer>().materials[i.MaterialIndex].SetColor("_EmissionColor", Off);
                }
                if (i.obj.GetComponent<Light>() != null)
                {
                    i.obj.GetComponent<Light>().color = Off;
                }
            }
        }
    }
}
