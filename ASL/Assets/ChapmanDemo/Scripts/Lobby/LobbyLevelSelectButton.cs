using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelSelectButtonType
{
    LEFT = 0,
    RIGHT = 1,
    NUM_BUTTON_TYPE = 2
};

public class LobbyLevelSelectButton : MonoBehaviour
{
    public GameObject LobbyPrefab; // Must contain ASLObject Component
    public LevelSelectButtonType buttonType = LevelSelectButtonType.NUM_BUTTON_TYPE;

    private void ClickButton()
    {
        if (buttonType == LevelSelectButtonType.NUM_BUTTON_TYPE) return;
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 20f))
        {
            if (hit.collider.gameObject.tag == "LobbyLevelSelectButton")
            {
                //Debug.Log("Click");
                //Debug.Log("name = " + hit.collider.gameObject.name);
                ASL.ASLObject asl = LobbyPrefab.GetComponent<ASL.ASLObject>();
                if (asl == null)
                    Debug.Log("Empty ASL Object in LobbyPrefab");
                float value = (float)(int)buttonType;
                float[] flr = new float[1];
                flr[0] = value + 1f;
                if (hit.collider.gameObject.name == "ArrowRight" && this.gameObject.name == "ArrowRight")
                {
                    LobbyPrefab.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                    {
                        LobbyPrefab.GetComponent<ASL.ASLObject>().SendFloatArray(flr);
                    });
                }
                else if (hit.collider.gameObject.name == "ArrowLeft" && this.gameObject.name == "ArrowLeft")
                {
                    LobbyPrefab.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                    {
                        LobbyPrefab.GetComponent<ASL.ASLObject>().SendFloatArray(flr);
                    });
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickButton();
        }
    }
}
