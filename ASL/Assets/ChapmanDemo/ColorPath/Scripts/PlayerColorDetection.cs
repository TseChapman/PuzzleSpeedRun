using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorDetection : MonoBehaviour
{
    private Color m_pathColor = Color.white;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject obj = hit.gameObject;
        //Debug.Log("Step on: " + obj.name);
        if (obj.tag == "ColorPad" && obj.name == "RedColorPad")
        {
            m_pathColor = Color.red;
        }
        else if (obj.tag == "ColorPad" && obj.name == "GreenColorPad")
        {
            m_pathColor = Color.green;
        }
        else if (obj.tag == "ColorPad" && obj.name == "BlueColorPad")
        {
            m_pathColor = Color.blue;
        }
        else if (obj.tag == "ColorPad" && obj.name == "YellowColorPad")
        {
            m_pathColor = Color.yellow;
        }
        else if (obj.tag == "ColorPad" && obj.name == "ResetColorPad")
        {
            m_pathColor = Color.white;
        }

        if (obj.tag == "ColorCube")
        {
            //Debug.Log("OnPath");
            obj.GetComponent<ASL.ASLObject>().SendAndSetClaim(() => 
            {
                obj.GetComponent<ASL.ASLObject>().SendAndSetObjectColor(m_pathColor, m_pathColor);
            });
        }
    }
}
