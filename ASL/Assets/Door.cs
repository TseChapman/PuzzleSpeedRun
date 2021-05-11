using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class Door : MonoBehaviour
{

    public float OpenOffset;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    // Start is called before the first frame update
    void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = transform.localPosition + new Vector3(0, OpenOffset, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Raise()
    {
        GetComponent<ASLObject>().SendAndSetClaim(() =>
        {
            GetComponent<ASLObject>().SendAndSetLocalPosition(openPosition);
        });
    }

    public void Lower()
    {
        GetComponent<ASLObject>().SendAndSetClaim(() =>
        {
            GetComponent<ASLObject>().SendAndSetLocalPosition(closedPosition);
        });
    }
}
