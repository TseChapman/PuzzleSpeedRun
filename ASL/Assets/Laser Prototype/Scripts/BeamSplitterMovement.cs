﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSplitterMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = 2.0f * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0.0f, 0.0f, -1f * distance);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0.0f, 0.0f, distance);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1f * distance, 0.0f, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(distance, 0.0f, 0f);
        }
    }
}
