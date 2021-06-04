using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class VRToggleUI : MonoBehaviour
{
    public InputActionAsset vRInputActionMap;
    private InputAction UIkeyLeft;
    private InputAction UIkeyRight;
    public bool UIShown = false;
    public GameObject canvasPanel;
    // Start is called before the first frame update
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
}
