using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LaserMirrorSelector : MonoBehaviour
{
    private bool hovering = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<LineRenderer>().forceRenderingOff = !hovering;
    }

    public void SelectEntered()
    {
    }
    public void SelectExited()
    {

    }

    public void HoverEntered()
    {
        hovering = true;
    }

    public void HoverExited()
    {
        hovering = false;
    }
}
