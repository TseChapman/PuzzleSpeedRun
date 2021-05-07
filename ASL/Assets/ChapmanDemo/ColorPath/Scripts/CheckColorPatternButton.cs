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

    private void CheckColorPathAnswer()
    {
        if (m_isColorPathFinished) return;
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 3f))
        {
            if (hit.collider.gameObject.name == "CheckColorPatternButton")
            {
                // Get result from checking answer
                bool isCorrect = colorPathSysten.CheckAnswer();
                Debug.Log("CheckColorPatternButton: isCorrect = " + isCorrect.ToString());
                // Set isColorPathFinished to the result of checking answer
                m_isColorPathFinished = isCorrect;
                float[] flr = new float[1];
                flr[0] = isCorrect ? 0 : 1f;
                this.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() => 
                {
                    this.gameObject.GetComponent<ASL.ASLObject>().SendFloatArray(flr);
                });

                // If answer is correct, destroy the exit door
                if (isCorrect)
                {
                    exitDoor.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                    {
                        exitDoor.GetComponent<ASL.ASLObject>().DeleteObject();
                    });
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckColorPathAnswer();
        }
    }
}
