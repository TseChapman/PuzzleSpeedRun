using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class VRToggleUI : MonoBehaviour
{
    public InputActionAsset vRInputActionMap;
    private InputAction UIkeyLeft;
    private InputAction UIkeyRight;
    public bool UIShown = false;
    public GameObject canvasPanel;
    public GameObject vrObject;
    public TMP_Text timerText;
    public float timeValue = 0;
    // Start is called before the first frame update

    void Update()
    {
        timeValue += Time.deltaTime;
        toggleTimer();

    }
    void Start()
    {
        var gameplayActionMap = vRInputActionMap.FindActionMap("XRI LeftHand");
        UIkeyLeft = gameplayActionMap.FindAction("UI Open");
        UIkeyLeft.performed += OnUIPress;
        UIkeyLeft.Enable();

        gameplayActionMap = vRInputActionMap.FindActionMap("XRI RightHand");
        UIkeyRight = gameplayActionMap.FindAction("UI Open");
        UIkeyRight.performed += OnUIPress;
        UIkeyRight.Enable();
    }

    void OnUIPress(InputAction.CallbackContext context)
    {
        if (UIShown)
        {
            canvasPanel.SetActive(false);
            UIShown = false;
        }
        else
        {
            canvasPanel.SetActive(true);
            UIShown = true;
        }
    }

    public void toggleTimer()
    {
        if (vrObject.transform.position.x <= 20 || vrObject.transform.position.z <= 20)
        {
            timeValue = 0;
            DisplayTime(timeValue);
        }
        else
        {
            DisplayTime(timeValue);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
