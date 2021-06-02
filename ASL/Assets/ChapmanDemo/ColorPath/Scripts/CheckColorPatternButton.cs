using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckColorPatternButton : MonoBehaviour
{
    public ColorPathSystem colorPathSysten;
    public GameObject exitDoor;
    private static bool m_isColorPathFinished = false;

    /// <param name="_myFloats">My float 4 array</param>
    /// <param name="_id">The id of the object that called <see cref="ASL.ASLObject.SendFloatArray_Example(float[])"/></param>
    public static void FloatCallback(string _id, float[] _floatArr)
    {
        m_isColorPathFinished = _floatArr[0] == 0 ? true : false;
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.gameObject.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(FloatCallback);
    }

    private void checkAnswer()
    {
        // Get result from checking answer
        bool isCorrect = colorPathSysten.CheckAnswer();
        Debug.Log("CheckColorPatternButton: isCorrect = " + isCorrect.ToString());
        // If answer is correct, destroy the exit door
        if (isCorrect && exitDoor != null && exitDoor.GetComponent<ASL.ASLObject>() != null)
        {
            Debug.Log("Removing color path door");
            exitDoor.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                exitDoor.GetComponent<ASL.ASLObject>().DeleteObject();
            });
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //if (m_isColorPathFinished) return;
            RaycastHit hit;
            Debug.Log("Raycasting from color path button script");
            var ray = MainCameraTracker.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 3f))
            {
                Debug.Log("Raycast hit from color path button script");
                if (hit.collider.gameObject.name == "CheckColorPatternButton")
                {
                    Debug.Log("Checking Answer from PC");
                    checkAnswer();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {   
        Debug.Log("Color path button hit by: " + other.gameObject.name);
        if (other.gameObject.layer == 18)
        {
            Debug.Log("Checking Answer from VR");
            checkAnswer();
        }
    }
}
