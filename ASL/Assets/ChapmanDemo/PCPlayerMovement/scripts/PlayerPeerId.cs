using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerPeerId : MonoBehaviour
{
    [SerializeField] public int id = -1;
    [SerializeField] private int m_peerId = -1;
    private bool m_isCallBackSet = false;

    public void SetPeerId(int _id)
    {
        id = _id;
        float[] flr = new float[1];
        flr[0] = (float)_id;
        Debug.Log("Client side: PlayerPeerId: SetPeerId(): id = " + _id);// + " Player Username: " + GameLiftManager.GetInstance().m_Players[id]);
        this.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.gameObject.GetComponent<ASL.ASLObject>().SendFloatArray(flr);
        });
    }

    public int GetPeerId() { return m_peerId; }

    public void SetCallBack()
    {
        if (m_isCallBackSet) return;
        Debug.Log("PlayerPeerId: SetCallBack(): Set callback function");
        this.gameObject.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(FloatCallback);
        m_isCallBackSet = true;
    }

    /// <param name="_myFloats">My float 4 array</param>
    /// <param name="_id">The id of the object that called <see cref="ASL.ASLObject.SendFloatArray_Example(float[])"/></param>
    public void FloatCallback(string _id, float[] _floatArr)
    {
        float value = _floatArr[0];
        m_peerId = (int)value;
        //Debug.Log("PlayerPeerId: Set Peer id = " + GameLiftManager.GetInstance().m_Players[m_peerId] + ", id = " + m_peerId);
        Debug.Log("PlayerPeerId: Set id on obj = " + _id);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        SetCallBack();
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
