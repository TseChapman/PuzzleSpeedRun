using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPeerId : MonoBehaviour
{
    private static int m_peerId = -1;

    public int GetPeerId() { return m_peerId; }

    /// <param name="_myFloats">My float 4 array</param>
    /// <param name="_id">The id of the object that called <see cref="ASL.ASLObject.SendFloatArray_Example(float[])"/></param>
    public static void FloatCallback(string _id, float[] _floatArr)
    {
        float value = _floatArr[0];
        m_peerId = (int)value;
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.gameObject.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(FloatCallback);
    }

    // Update is called once per frame
    private void Update()
    {
        return;
    }
}
