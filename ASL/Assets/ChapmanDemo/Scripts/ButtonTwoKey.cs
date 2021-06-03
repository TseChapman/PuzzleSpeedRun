using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTwoKey : MonoBehaviour
{
    public GameObject keyOne;
    public GameObject keyTwo;
    public GameObject[] keys = new GameObject[10];
    public Color keyColor;
    public Color keyTwoColor;
    public Color buttonColor;
    private float timeElapsed = 0f;
    public Transform input;
    bool animation = false;
    bool clickable = true;
    //different button orreintation
    public bool posX = false;
    public bool negX = false;
    public bool posZ = false;
    public bool negZ = false;


    bool button1 = false;
    public int handLayer = 18;
    private bool isClickedWithVR = false;
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
        }else if (Input.GetMouseButtonDown(0) && clickable)
        {
            int layerMask = 1 << 10;
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2, layerMask))
            {
                if (hit.transform.GetComponent<Renderer>().material.color == buttonColor)
                {

                    turnOn(hit.transform);

                }
            }
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
            }
            else if (negX)
            {
                timeElapsed += Time.deltaTime;
                moveX(-0.5f * Time.deltaTime, input);
            }
            else if (posZ)
            {
                timeElapsed += Time.deltaTime;
                moveZ(0.5f * Time.deltaTime, input);
            }
            else if (negZ)
            {
                timeElapsed += Time.deltaTime;
                moveZ(-0.5f * Time.deltaTime, input);
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
            else if (posZ)
            {
                timeElapsed += Time.deltaTime;
                moveZ(-0.5f * Time.deltaTime, input);
            }
            else if (negZ)
            {
                timeElapsed += Time.deltaTime;
                moveZ(0.5f * Time.deltaTime, input);
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
    public void moveZ(float addition, Transform input)
    {
        input.localPosition += new Vector3(0, 0, addition);
    }

    public void resetKeys()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] != null)
            {
                keys[i].GetComponent<Renderer>().material.color = Color.white;
                keys[i].GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                {
                    keys[i].GetComponent<ASL.ASLObject>().SendAndSetObjectColor(Color.white, Color.white);
                });
            }
        }

    }

    private void turnOn(Transform buttonTransform)
    {
            if (keyOne.GetComponent<Renderer>().material.color == keyColor)
            {
                animation = true;
                clickable = false;
                input = buttonTransform.transform;
                if (!button1)
                {
                        keyTwo.GetComponent<Renderer>().material.color = keyTwoColor;
                        keyTwo.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                        {
                            keyTwo.GetComponent<ASL.ASLObject>().SendAndSetObjectColor(keyTwoColor, keyTwoColor);
                        });
                        button1 = true;
                }
                else
                {
                        keyTwo.GetComponent<Renderer>().material.color = Color.white;
                        keyTwo.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                        {
                            keyTwo.GetComponent<ASL.ASLObject>().SendAndSetObjectColor(Color.white, Color.white);
                        });
                        button1 = false;
                }
            }
            else
            {
                 resetKeys();
            }
    }

    public void setKey(GameObject input)
    {
        keyOne = input;
    }

    public void setKey2(GameObject input)
    {
        keyTwo = input;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRY CLICK!");
        if (other.gameObject.layer == handLayer)
        {
            turnOn(transform);
        }
    }
}
