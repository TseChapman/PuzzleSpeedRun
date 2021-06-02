using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelUI : MonoBehaviour
{
    public float timeValue = 0;
    public Text timerText;
    public GameObject panel;
    public GameObject mainPanel;
    public GameObject controlsPanel;

    void Update()
    {
        timeValue += Time.deltaTime;
        DisplayTime(timeValue);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            TogglePanel();
        }

    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void TogglePanel()
    {
        if(panel != null)
        {
            bool isActive = panel.activeSelf;

            panel.SetActive(!isActive);
           
        }
    }

    public void TogglePanelClick()
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;

            panel.SetActive(!isActive);
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void ToggleControlsPanelClick()
    {
        bool isActive = mainPanel.activeSelf;

        mainPanel.SetActive(!isActive);

        isActive = controlsPanel.activeSelf;

        controlsPanel.SetActive(!isActive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
