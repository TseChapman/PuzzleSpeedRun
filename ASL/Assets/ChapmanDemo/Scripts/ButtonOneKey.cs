using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class ButtonOneKey : MonoBehaviour
{
    public GameObject keyOne;
    public Color keyColor;
    public Color buttonColor;
    private float timeElapsed = 0f;
    public Transform input;
    bool animation = false;
    bool clickable = true;

    //different button orreintation
    public bool posX = false;
    public bool negX = false;

    bool button1 = false;
    public int handLayer = 18;
    //private bool isClickedWithVR = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (animation)
        {
            buttonAnimation(input.transform);
        } else if (Input.GetMouseButtonDown(0) && clickable)
        {
            int layerMask = 1 << 10;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, layerMask))
            {
                if (hit.transform.GetComponent<Renderer>().material.color == buttonColor)
                {
                    animation = true;
                    clickable = false;
                    input = hit.transform;
                    turnOn();

                }
            }
        }
    }
    private void turnOn()
    {
        if (!button1)
        {
            keyOne.GetComponent<Renderer>().material.color = keyColor;
            keyOne.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                Debug.Log("hi");
                keyOne.GetComponent<ASL.ASLObject>().SendAndSetObjectColor(keyColor, keyColor);
            });
            button1 = true;
        }
        else
        {
            keyOne.GetComponent<Renderer>().material.color = Color.white;
            keyOne.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                keyOne.GetComponent<ASL.ASLObject>().SendAndSetObjectColor(Color.white, Color.white);
            });
            button1 = false;
        }

    }

    public void buttonAnimation(Transform input)
    {
        if (timeElapsed < 0.1)
        {
            if (posX)
            {
                timeElapsed += Time.deltaTime;
                moveX(0.5f * Time.deltaTime, input);
            }else if (negX)
            {
                timeElapsed += Time.deltaTime;
                moveX(-0.5f * Time.deltaTime, input);
            }
        }
        else if (timeElapsed < 0.211)
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
            animation = false;
            clickable = true;
            timeElapsed = 0f;
        }
    }

    public void moveX(float addition, Transform input)
    {
        input.localPosition += new Vector3(addition, 0, 0);
    }

    public void setKey(GameObject input) 
    {
        keyOne = input;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("TRY CLICK!");
         if (other.gameObject.layer == handLayer)
            
        {
                turnOn();
        }
    }
}
