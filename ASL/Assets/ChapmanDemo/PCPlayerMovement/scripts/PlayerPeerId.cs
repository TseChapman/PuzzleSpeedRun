using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerPeerId : MonoBehaviour
{
    [SerializeField] private int m_peerId = -1;
    public string playerName = "";
    private int prevId = -1;
    private ASL.ASLObject m_aslObject;
    private bool m_isCallBackSet = false;

    public bool GetIsCallBackSet() { return m_isCallBackSet; }

    public void SetPeerId(int _id)
    {
        float[] flr = new float[1];
        flr[0] = (float)_id;
        //Debug.Log("Client side: PlayerPeerId: SetPeerId(): id = " + _id);// + " Player Username: " + GameLiftManager.GetInstance().m_Players[id]);
        m_aslObject.SendAndSetClaim(() =>
        {
            m_aslObject.SendFloatArray(flr);
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
        Debug.Log("PlayerPeerId: Set Peer id = " + GameLiftManager.GetInstance().m_Players[m_peerId] + ", id = " + m_peerId);
        Debug.Log("PlayerPeerId: Set id on obj = " + _id);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        m_aslObject = this.gameObject.GetComponent<ASL.ASLObject>();
        SetCallBack();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_isCallBackSet)
        {
            SetCallBack();
        }
        if (m_peerId != -1 && m_peerId != prevId)
        {
            float[] flr = new float[1];
            flr[0] = (float)m_peerId;
            //Debug.Log("Client side: PlayerPeerId: Update(): id = " + _id);// + " Player Username: " + GameLiftManager.GetInstance().m_Players[id]);
            m_aslObject.SendAndSetClaim(() =>
            {
                m_aslObject.SendFloatArray(flr);
            });
            prevId = m_peerId;
        }
        if (m_peerId != -1)
        {
            playerName = GameLiftManager.GetInstance().m_Players[m_peerId];
        }
    }
}
