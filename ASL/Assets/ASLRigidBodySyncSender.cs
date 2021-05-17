using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class ASLRigidBodySyncSender : MonoBehaviour
{

    private bool initialized = false;

    public class SyncInfo {
        public Vector3 position
        {
            get { return mPosition; }
        }

        public Vector3 inertiaTensor
        {
            get { return mInertiaTensor; }
        }

        public Quaternion rotation
        {
            get { return mRotation; }
        }

        public Quaternion inertiaTensorRotation
        {
            get { return mInertiaTensorRotation; }
        }

        public SyncInfo()
        {

        }

        public SyncInfo(Rigidbody rb)
        {
            mPosition = rb.position;
            mRotation = rb.rotation;
            mInertiaTensor = rb.inertiaTensor;
            mInertiaTensorRotation = rb.inertiaTensorRotation;
        }

        public SyncInfo(float[] data, int offset)
        {
            mPosition.x = data[offset];
            mPosition.y = data[offset + 1];
            mPosition.z = data[offset + 2];
            mInertiaTensor.x = data[offset + 3];
            mInertiaTensor.y = data[offset + 4];
            mInertiaTensor.z = data[offset + 5];
            mRotation.x = data[offset + 6];
            mRotation.y = data[offset + 7];
            mRotation.z = data[offset + 8];
            mRotation.w = data[offset + 9];
            mInertiaTensorRotation.x = data[offset + 10];
            mInertiaTensorRotation.y = data[offset + 11];
            mInertiaTensorRotation.z = data[offset + 12];
            mInertiaTensorRotation.w = data[offset + 13];
        }

        public float[] asFloats()
        {
            float[] res = new float[14];
            res[0] = mPosition.x;
            res[1] = mPosition.y;
            res[2] = mPosition.z;
            res[3] = mInertiaTensor.x;
            res[4] = mInertiaTensor.y;
            res[5] = mInertiaTensor.z;
            res[6] = mRotation.x;
            res[7] = mRotation.y;
            res[8] = mRotation.z;
            res[9] = mRotation.w;
            res[10] = mInertiaTensorRotation.x;
            res[11] = mInertiaTensorRotation.y;
            res[12] = mInertiaTensorRotation.z;
            res[13] = mInertiaTensorRotation.w;
            return res;
        }

        public Vector3 mPosition;
        public Vector3 mInertiaTensor;
        public Quaternion mRotation;
        public Quaternion mInertiaTensorRotation;
    }

    private Dictionary<string, SyncInfo> syncInfos;

    private ulong fixedUpdateFrames = 0;
    private ulong updateFrames = 0;

    private int peerID;

    public ASLRigidBodySyncSender()
    {
        syncInfos = new Dictionary<string, SyncInfo>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
        {
            peerID = (int)transform.position.x;
            ASLRigidBodySync.AddSender(peerID, this);
            GetComponent<ASLObject>()._LocallySetFloatCallback(floatCallback);
            initialized = true;
        }
        if (updateFrames++ % 5 == 0 && peerID == GameLiftManager.GetInstance().m_PeerId)
        {
            GetComponent<ASLObject>().SendAndSetClaim(() => {
                foreach (string objectID in syncInfos.Keys)
                {
                    float[] data = new float[objectID.Length + 15];
                    char[] objectIDChars = objectID.ToCharArray();
                    data[0] = objectIDChars.Length;
                    for (int i = 1; i <= objectIDChars.Length; ++i)
                    {
                        data[i] = objectIDChars[i - 1];
                    }
                    syncInfos[objectID].asFloats().CopyTo(data, objectIDChars.Length + 1);
                    GetComponent<ASLObject>().SendFloatArray(data);
                }
            });
        }
    }
    
    public SyncInfo GetSyncInfo(string objectID)
    {
        if (!syncInfos.ContainsKey(objectID))
        {
            return null;
        }
        return syncInfos[objectID];
    }

    public void Send(string objectID, Rigidbody rigidBody)
    {
        syncInfos[objectID] = new SyncInfo(rigidBody);
    }

    void FixedUpdate()
    {

    }

    private void floatCallback(string id, float[] data)
    {
        string objectID = "";
        int objectIDLength = (int)data[0];
        for (int i = 0; i < objectIDLength; ++i)
        {
            objectID += (char)data[i + 1];
        }
        Debug.Log("FC: " + objectID);
        SyncInfo si = new SyncInfo(data, objectIDLength+1);
        syncInfos[objectID] = si;
    }

    private void recieve(string objectID, SyncInfo syncInfo)
    {
        syncInfos[objectID] = syncInfo;
    }

}
