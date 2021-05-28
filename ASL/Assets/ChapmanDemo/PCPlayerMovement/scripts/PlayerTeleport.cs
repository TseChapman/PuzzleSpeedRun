using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private static Vector3 pos;
    private static bool isSignalTeleport = false;

    public bool GetIsSignalTeleport() { return isSignalTeleport; }

    public void ResetSignal() { isSignalTeleport = false; }

    public Vector3 GetPos()
    {
        Debug.Log(pos);
        return pos;
    }

    /// <param name="_myFloats">My float 4 array</param>
    /// <param name="_id">The id of the object that called <see cref="ASL.ASLObject.SendFloatArray_Example(float[])"/></param>
    public static void FloatCallback(string _id, float[] _floatArr)
    {
        //if (_floatArr.Length != 3) return;
        //Debug.Log("Received callback");
        float x = _floatArr[0];
        float y = _floatArr[1];
        float z = _floatArr[2];
        Vector3 newPos = Vector3.zero;
        newPos.x = x;
        newPos.y = y;
        newPos.z = z;
        pos = newPos;
        isSignalTeleport = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        pos = this.gameObject.transform.position;
        this.gameObject.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(FloatCallback);
    }
}
