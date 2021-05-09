using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PressurePlate : MonoBehaviour
{
    public GameObject door = null;
    bool onPressurePlate = false;
    public LayerMask m_LayerMask;

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
        if (hitColliders.Length > 0)
            onPressurePlate = true;
        else
            onPressurePlate = false;
        if (onPressurePlate && door.transform.position.y <= 2.6)
        {
            door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y + (float)5, door.transform.position.z);
            //Just set position
            door.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                door.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(door.transform.position);
            });
        }
        else if(!onPressurePlate && door.transform.position.y > 2.5)
        {
            door.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - (float)5, door.transform.position.z);
            //Just set position
            door.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                door.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(door.transform.position);
            });
        }

    }
/*
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("plate pressed!");
        onPressurePlate = true;
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("plate release!");
        onPressurePlate = false;
    }
    */
}
