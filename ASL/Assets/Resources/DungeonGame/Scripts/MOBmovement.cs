using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOBmovement : MonoBehaviour
{
    public Transform target;

    float mobSpeed = 2f;

    const float eps = 2f;
    // Start is called before the first frame update
    void Start()
    {
       target = GameObject.Find("PCPlayer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.position);

        if((transform.position - target.position).magnitude > eps)
        {
            transform.Translate(0f, 0f, mobSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
       // fall();
    }

   
}
