using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawn : MonoBehaviour
{
    public GameObject button;
    public GameObject ball;
    public GameObject spawnArea;
    public Transform input;
    public float startPosX;
    private float timeElapsed = 0f;
    bool animation = false;
    bool clickable = true;

    //different button orreintation
    public bool posX = false;
    public bool negX = false;
    // Update is called once per frame
    private void Start()
    {
        startPosX = transform.localPosition.x;
    }

    void Update()
    {
        if (animation)
        {
            buttonAnimation(input.transform);
        }
        else if (Input.GetMouseButtonDown(0) && clickable)
        {   int layerMask = 1 << 10;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3, layerMask))
            {
                if (hit.transform == button.transform)
                {
                    animation = true;
                    clickable = false;
                    input = hit.transform;
                    ball.transform.position = spawnArea.transform.position;
                }

            }
        }
    }

    private void turnOn()
    {
            animation = true;
            clickable = false;
            input = transform;
            ball.transform.position = spawnArea.transform.position;

    }

    public void buttonAnimation(Transform input)
    {
        if (timeElapsed < 0.1)
        {
            timeElapsed += Time.deltaTime;
            moveZ(0.5f * Time.deltaTime, input);

        }
        else if (timeElapsed < 0.211)
        {
            timeElapsed += Time.deltaTime;
            moveZ(-0.5f * Time.deltaTime, input);

        }
        else
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > 0.55)
            {
                input.localPosition = new Vector3(startPosX, input.localPosition.y, input.localPosition.z);
                animation = false;
                clickable = true;
                timeElapsed = 0f;
            }
        }
    }

    public void moveZ(float addition, Transform input)
    {
        input.localPosition += new Vector3(0, 0, addition);
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
