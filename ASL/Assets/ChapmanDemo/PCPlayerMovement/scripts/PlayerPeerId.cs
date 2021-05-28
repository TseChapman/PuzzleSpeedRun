using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerPeerId : MonoBehaviour
{
    [SerializeField] public int id = -1;
    [SerializeField] private int m_peerId = -1;

    public void SetPeerId(int _id)
    {
        id = _id;
        float[] flr = new float[1];
        flr[0] = (float)_id;
        this.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.gameObject.GetComponent<ASL.ASLObject>().SendFloatArray(flr);
        });
    }

    public int GetPeerId() { return m_peerId; }

    /// <param name="_myFloats">My float 4 array</param>
    /// <param name="_id">The id of the object that called <see cref="ASL.ASLObject.SendFloatArray_Example(float[])"/></param>
    public void FloatCallback(string _id, float[] _floatArr)
    {
        float value = _floatArr[0];
        m_peerId = (int)value;
        Debug.Log("PlayerPeerId: Set Peer id = " + GameLiftManager.GetInstance().m_Players[m_peerId] + ", id = " + m_peerId);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        this.gameObject.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(FloatCallback);
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (id == -1 && id != m_peerId)
            id = m_peerId;
        /*
        if (id != m_peerId)
            m_peerId = id;
        */
    }
}
