using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class ASLRigidBodySync : MonoBehaviour
{


    // Should be set by ASLRigidBodySyncSender script
    private static ASLRigidBodySyncSender senderObject = null;
    private static Dictionary<int, ASLRigidBodySyncSender> recievers;

    private static bool initialized = false;

    private ulong fixedUpdateFrames = 0;

    public ASLRigidBodySync()
    {
        recievers = new Dictionary<int, ASLRigidBodySyncSender>();
    }

    public static void AddSender(int peerID, ASLRigidBodySyncSender sender)
    {
        Debug.Log("AddSender: peerID="+peerID + " ID: "+ sender.GetComponent<ASLObject>().m_Id + ".");
        if (peerID == GameLiftManager.GetInstance().m_PeerId)
        {
            senderObject = sender;
        } else
        {
            recievers.Add(peerID, sender);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (GetComponent<ASLObject>() == null || senderObject == null)
        {
            return;
        }
        string objectID = GetComponent<ASLObject>().m_Id;

        // average transform, phys info from the other machines
        Vector3 pos = new Vector3(0, 0, 0);
        Vector3 inertia = new Vector3(0, 0, 0);
        Quaternion rotation = Quaternion.identity;
        Quaternion rotationInertia = Quaternion.identity;
        int count = 0;
        foreach (ASLRigidBodySyncSender reciever in recievers.Values)
        {
            ASLRigidBodySyncSender.SyncInfo syncInfo = reciever.GetSyncInfo(objectID);
            if (syncInfo == null)
            {
                continue;
            }
            pos += syncInfo.position;
            inertia += syncInfo.inertiaTensor;

            count++;
        }

        Rigidbody rb = GetComponent<Rigidbody>();

        if (count > 0)
        {
            pos /= count;
            inertia /= count;

            //Debug.Log("pos=" + pos);
            float fade = 0.5f;

            rb.position = rb.position * fade + pos * (1 - fade);

            Vector3 nextInertiaTensor = rb.inertiaTensor * fade + inertia * (1 - fade);
            if (nextInertiaTensor.x != 0 && nextInertiaTensor.y != 0 && nextInertiaTensor.z != 0)
            {
                rb.inertiaTensor = nextInertiaTensor;
            }

            foreach (ASLRigidBodySyncSender reciever in recievers.Values)
            {
                ASLRigidBodySyncSender.SyncInfo syncInfo = reciever.GetSyncInfo(objectID);
                if (syncInfo == null)
                {
                    continue;
                }
                rb.rotation = Quaternion.Slerp(rb.rotation, syncInfo.rotation, (1 - fade) / count);
                rb.inertiaTensorRotation = Quaternion.Slerp(rb.inertiaTensorRotation, syncInfo.inertiaTensorRotation, (1 - fade) / count);
            }

            // every sync timestep
            // send peer id, object id, transform, phys info via senderobject
            // sender object can buffer and send all at updates at once (will need to be set in script ordering!)
        }
        if (fixedUpdateFrames++ % 1 == 0)
        {
            int peerID = GameLiftManager.GetInstance().m_PeerId;
            senderObject.Send(objectID, rb);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized && GameLiftManager.GetInstance() != null)
        {
            ASLHelper.InstantiateASLObject("ASLRigidBodySyncSender", new Vector3(GameLiftManager.GetInstance().m_PeerId, 0, 0), Quaternion.identity);
            initialized = true;
        }
    }
}
