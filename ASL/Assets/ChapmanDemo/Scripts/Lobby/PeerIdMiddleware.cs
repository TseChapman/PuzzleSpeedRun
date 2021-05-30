using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PeerIdMiddleware : MonoBehaviour
{
    private bool m_isAllMeshSetCallback; // This means all client's PlayerMesh's Child 1 PlayerPeerId callback is set
    private int numClientCallBackSet = 0;
    private bool m_isLocalCallBackSet = false;
    private float m_timer = 1f;

    public void SendSetCallBack(int clientId)
    {
        float[] flr = new float[1];
        flr[0] = (float)clientId;
        this.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.gameObject.GetComponent<ASL.ASLObject>().SendFloatArray(flr);
        });
    }

    public bool GetIsLocalCallBackSet() 
    {
        if (m_timer > 0) return false;
        return m_isLocalCallBackSet;
    }

    public bool IsAllClientSet()
    {
        //Debug.Log("numClientSet = " + numClientCallBackSet + ", Num player = " + GameLiftManager.GetInstance().m_Players.Count);
        return (numClientCallBackSet == GameLiftManager.GetInstance().m_Players.Count);
    }

    private void FloatCallBack(string _id, float[] _float)
    {
        int clientId = (int)_float[0];
        Debug.Log("Client Id = " + clientId + " set all callback");
        numClientCallBackSet++;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        this.gameObject.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(FloatCallBack);
        m_isLocalCallBackSet = true;
        //Debug.Log("m_isLocalCallBackSet = true");
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_isLocalCallBackSet)
            m_timer -= Time.smoothDeltaTime;

    }
}
