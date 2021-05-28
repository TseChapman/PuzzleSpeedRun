using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamLighter : MonoBehaviour
{
    public GameObject LightSource;
    public float LightDensity;
    public float Intensity;
    public float ClipDistance;
    private List<GameObject> lights;

    public bool ConstantLightCount;
    public int LightCount;

    public LaserSensor ExcludeSensor;

    public bool CanTriggerSensors;

    // Start is called before the first frame update
    void Start()
    {
        lights = new List<GameObject>();
    }

    void FixedUpdate()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.TransformDirection(new Vector3(0f, 0f, 1f)));
        float nonSensorHitDistance = -1;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~LayerMask.GetMask("LaserSensor") & ~LayerMask.GetMask("BoundaryTrigger") & ~LayerMask.GetMask("VRHandle")))
        {
            nonSensorHitDistance = hit.distance;
            LaserSensor sensor = hit.transform.gameObject.GetComponent<LaserSensor>();
            if (sensor != null && sensor != ExcludeSensor && CanTriggerSensors)
            {
                sensor.Trigger(ray, hit);
            }
            Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0f, 0f, 1f)) * hit.distance, Color.yellow);
            transform.localScale = new Vector3(1f, 1f, Mathf.Min(ClipDistance, hit.distance));
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0f, 0f, 1f)) * 1000, Color.white);
            // Vector3(0,-0.495000005,-0.600000024)
            transform.localScale = new Vector3(1f, 1f, ClipDistance);
        }
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("LaserSensor") & ~LayerMask.GetMask("BoundaryTrigger") & ~LayerMask.GetMask("VRHandle")))
        {
            if (hit.distance <= nonSensorHitDistance)
            {
                LaserSensor sensor = hit.transform.gameObject.GetComponent<LaserSensor>();
                Debug.Log(hit.transform.gameObject.name);
                if (sensor != null && sensor != ExcludeSensor && CanTriggerSensors)
                {
                    sensor.Trigger(ray, hit);
                }
                Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0f, 0f, 1f)) * hit.distance, Color.green);
            }
        }

        float dist = transform.localScale.z;
        dist = Mathf.Min(ClipDistance, dist);

        int lightCount = (int)(dist * LightDensity);
        if (lightCount < 0)
        {
            lightCount = 0;
        }

        if (ConstantLightCount)
        {
            lightCount = LightCount;
            LightDensity = lightCount / dist;
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
            light.transform.localPosition = new Vector3(0.0f, 0.0f, i/LightDensity/dist + 0.5f / lightCount);
            light.GetComponent<Light>().intensity = Intensity / LightDensity;
        }
    }
}
