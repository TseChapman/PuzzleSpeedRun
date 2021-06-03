using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMirror : MonoBehaviour
{
    public GameObject LaserPrefab;
    public int MaxReflections;

    private List<GameObject> ReflectedLasers;

    // Start is called before the first frame update
    void Start()
    {
        ReflectedLasers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        foreach (LaserSensor.Hit hit in GetComponent<LaserSensor>().ActiveHits)
        {
            if (i >= MaxReflections)
            {
                break;
            }
            GameObject laser = ReflectedLasers[i];
            laser.transform.position = hit.RaycastHit.point;
            Vector3 normal = hit.RaycastHit.normal;
            Vector3 v = hit.Ray.direction.normalized;
            Vector3 Reflected = v - 2.0f * Vector3.Dot(v, normal) * normal;
            laser.transform.forward = Reflected;

            i++;
        }
    }

    public void OnHitChange(int hitCount)
    {
        // Make sure ReflectedLasers has one laser prefab per detected laser beam
        while (ReflectedLasers.Count < hitCount && ReflectedLasers.Count <= MaxReflections)
        {
            ReflectedLasers.Add(GameObject.Instantiate(LaserPrefab));
        }
        while (ReflectedLasers.Count > hitCount && ReflectedLasers.Count <= MaxReflections)
        {
            GameObject toRemove = ReflectedLasers[0];
            ReflectedLasers.RemoveAt(0);
            Destroy(toRemove);
        }
    }

    public void OnDestroy()
    {
        for (int i = ReflectedLasers.Count - 1; i >= 0; --i) {
            GameObject obj = ReflectedLasers[i];
            ReflectedLasers.RemoveAt(i);
            Destroy(obj);
        }
    }
}
