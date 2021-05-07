using ASL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASLMovingPlatformSync : MonoBehaviour
{
    private Vector3 remotePosition;
    private Quaternion remoteRotation;
    private Vector3 remoteScale;
    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private Vector3 previousScale;
    private float remoteSpeed;
    private float remoteRelativePosition;
    private float previousSpeed;
    private float previousRelativePosition;
    private bool transformChangedRemotely;
    private int id;
    private int lockHolder;
    private bool madeInitialClaim;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ASLObject>()._LocallySetFloatCallback((string _id, float[] f) =>
        {
            lockHolder = id;
            if (id != f[0])
            {
                remotePosition = new Vector3(f[1], f[2], f[3]);
                remoteRotation = new Quaternion(f[4], f[5], f[6], f[7]);
                remoteScale = new Vector3(f[8], f[9], f[10]);
                remoteSpeed = f[11];
                remoteRelativePosition = f[12];
                transformChangedRemotely = true;
            }
        });
    }

    void FixedUpdate()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().isKinematic = !GetComponent<ASLObject>().m_Mine;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameLiftManager glm = GameLiftManager.GetInstance();
        if (glm == null)
        {
            return;
        }
        List<string> usernames = new List<string>();
        foreach (string username in glm.m_Players.Values)
        {
            usernames.Add(username);
        }
        usernames.Sort();
        id = usernames.IndexOf(glm.m_Username);

        if (id == 0 && !madeInitialClaim)
        {
            GetComponent<ASLObject>().SendAndSetClaim(() => { });
            madeInitialClaim = true;
        }
        // if transform != previousTransform then transform has changed locally, so
        //     send transform data in float array to be stored in remoteTransform
        // load remoteTransform into local transform if remoteTransform recieved
        // store transform into previousTransform

        if (transform.localPosition != previousPosition || transform.localRotation != previousRotation || transform.localScale != previousScale)
        {
            // position changed locally
            GetComponent<ASLObject>().SendAndSetClaim(() =>
            {
                string fromID = GetComponent<MovingPlatform>().From.gameObject.GetComponent<ASLObject>().m_Id;
                string toID = GetComponent<MovingPlatform>().To.gameObject.GetComponent<ASLObject>().m_Id;

                float[] f = new float[13 + fromID.Length + toID.Length + 2];

                int idx = 0;

                f[idx++] = id;
                f[idx++] = transform.localPosition.x;
                f[idx++] = transform.localPosition.y;
                f[idx++] = transform.localPosition.z;

                f[idx++] = transform.localRotation.x;
                f[idx++] = transform.localRotation.y;
                f[idx++] = transform.localRotation.z;
                f[idx++] = transform.localRotation.w;

                f[idx++] = transform.localScale.x;
                f[idx++] = transform.localScale.y;
                f[idx++] = transform.localScale.z;

                f[idx++] = GetComponent<MovingPlatform>().Speed;
                f[idx++] = GetComponent<MovingPlatform>().RelativePosition;

                f[idx++] = fromID.Length;
                for (int i = idx; i < idx + fromID.Length; ++i)
                {
                    f[i] = fromID[i - idx];
                }
                idx += fromID.Length;

                f[idx++] = toID.Length;
                for (int i = idx; i < idx + toID.Length; ++i)
                {
                    f[i] = toID[i - idx];
                }

                GetComponent<ASLObject>().SendFloatArray(f);
            });
        }
        if (transformChangedRemotely)
        {
            transform.localPosition = remotePosition;
            transform.localRotation = remoteRotation;
            transform.localScale = remoteScale;
            transformChangedRemotely = false;
        }
        previousPosition = transform.localPosition;
        previousRotation = transform.localRotation;
        previousScale = transform.localScale;
    }
}
