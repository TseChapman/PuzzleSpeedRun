﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamLighter : MonoBehaviour
{
    public GameObject LightSource;
    public float LightDensity;
    public float Intensity;
    public float ClipDistance;
    private List<GameObject> lights;

    public LaserSensor ExcludeSensor;

    // Start is called before the first frame update
    void Start()
    {
        lights = new List<GameObject>();
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(0f, 0f, 1f)), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0f, 0f, 1f)) * hit.distance, Color.yellow);
            transform.localScale = new Vector3(1f, 1f, Mathf.Min(ClipDistance, hit.distance));
            LaserSensor sensor = hit.transform.gameObject.GetComponent<LaserSensor>();
            if (sensor != null && sensor != ExcludeSensor)
            {
                sensor.Trigger();
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0f, 0f, 1f)) * 1000, Color.white);
            // Vector3(0,-0.495000005,-0.600000024)
            transform.localScale = new Vector3(1f, 1f, ClipDistance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int lightCount = (int)(transform.localScale.z * LightDensity);
        if (lightCount < 0)
        {
            lightCount = 0;
        }
        if (lights.Count > lightCount)
        {
            for (int i = 0; i < lights.Count - lightCount; ++i)
            {
                GameObject toRemove = lights[0];
                lights.RemoveAt(0);
                Destroy(toRemove);
            }
        }
        while (lights.Count < lightCount)
        {
            lights.Add(GameObject.Instantiate(LightSource, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity));
        }
        for (int i = 0; i < lightCount; ++i)
        {
            GameObject light = lights[i];
            light.transform.SetParent(transform);
            light.transform.localPosition = new Vector3(0.0f, 0.0f, i/LightDensity/transform.localScale.z + 0.5f / lightCount);
            light.GetComponent<Light>().intensity = Intensity / LightDensity;
        }
    }
}
