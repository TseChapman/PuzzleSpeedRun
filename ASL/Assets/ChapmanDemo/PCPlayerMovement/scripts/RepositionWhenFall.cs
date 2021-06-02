using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionWhenFall : MonoBehaviour
{
    private Vector3 initialPosition;
    private void Start()
    {
        initialPosition = transform.position;
    }
    private void preventFalling()
    {
        transform.position = initialPosition;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    private void FixedUpdate()
    {
        if (transform.position.y < -20f) //is falling
        {
            preventFalling();
        }
    }

    public void isGrabbed()
    {
        Debug.Log("cube is grabbed?!?" );
    }
}
