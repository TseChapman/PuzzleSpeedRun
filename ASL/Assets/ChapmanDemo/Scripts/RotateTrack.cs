using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrack : MonoBehaviour
{
    public GameObject track;
    public GameObject button;
    private float timeElapsed = 0f;
    public Transform input;
    public float startPosX;
    bool animation = false;
    bool clickable = true;

    //different button orreintation
    public bool posX = false;
    public bool negX = false;
    // Update is called once per frame

    void Update()
    {
        if (animation)
        {
            buttonAnimation(input.transform);
        }
        else if (Input.GetMouseButtonDown(0) && clickable)
        {
            int layerMask = 1 << 10;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3, layerMask))
            {
                if(hit.transform == button.transform)
                {
                    animation = true;
                    clickable = false;
                    input = hit.transform;
                    track.transform.Rotate(0, 90, 0);
                    track.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                    {
                        track.GetComponent<ASL.ASLObject>().SendAndSetWorldRotation(track.transform.rotation);
                    });
                }
               
            }
        }
    }

    private void turnOn()
    {
        animation = true;
        clickable = false;
        input = transform;
        track.transform.Rotate(0, 90, 0);
        track.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            track.GetComponent<ASL.ASLObject>().SendAndSetWorldRotation(track.transform.rotation);
        });
    }


    public void buttonAnimation(Transform input)
    {
        if (timeElapsed < 0.1)
        {
            if (posX)
            {
                timeElapsed += Time.deltaTime;
                moveX(0.5f * Time.deltaTime, input);
            }
            else if (negX)
            {
                timeElapsed += Time.deltaTime;
                moveX(-0.5f * Time.deltaTime, input);
            }
        }
        else if (timeElapsed < 0.2115)
        {
            if (posX)
            {
                timeElapsed += Time.deltaTime;
                moveX(-0.5f * Time.deltaTime, input);
            }
            else if (negX)
            {
                timeElapsed += Time.deltaTime;
                moveX(0.5f * Time.deltaTime, input);
            }
        }
        else
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > 0.55)
            {
                input.localPosition = new Vector3(startPosX, input.position.y, input.position.z);
                animation = false;
                clickable = true;
                timeElapsed = 0f;
            }             
        }
    }

    public void moveX(float addition, Transform input)
    {
        input.localPosition += new Vector3(addition, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRY CLICK!");
        if (other.gameObject.layer == 18)
        {
            turnOn();
        }
    }
}
