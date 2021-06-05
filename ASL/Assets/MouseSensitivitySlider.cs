using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivitySlider : MonoBehaviour
{
    public GameObject pcPlayer;
    public GameObject camerao;
    public Slider mouseSens;
    MouseFirstPersonView personView;

    // Start is called before the first frame update
    void Start()
    {
        //camera = pcPlayer.transform.GetChild(0).gameObject;
        if (camerao != null)
        personView = camerao.GetComponent<MouseFirstPersonView>();
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
