using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivitySlider : MonoBehaviour
{
    public GameObject pcPlayer;
    public GameObject camera;
    public Slider mouseSens;
    MouseFirstPersonView personView;

    // Start is called before the first frame update
    void Start()
    {
        camera = pcPlayer.transform.GetChild(0).gameObject;
        personView = camera.GetComponent<MouseFirstPersonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void slider()
    {
        personView.updateMouseSens(mouseSens);
    }
}
