using ASL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASLTransformSync : MonoBehaviour
{
    private Vector3 remotePosition;
    private Quaternion remoteRotation;
    private Vector3 remoteScale;
    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private Vector3 previousScale;
    private bool transformChangedRemotely;
    private int id;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ASLObject>()._LocallySetFloatCallback((string _id, float[] f) =>
        {
            if (id != f[0])
            {
                remotePosition = new Vector3(f[1], f[2], f[3]);
                remoteRotation = new Quaternion(f[4], f[5], f[6], f[7]);
                remoteScale = new Vector3(f[8], f[9], f[10]);
                transformChangedRemotely = true;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        GameLiftManager glm = GameLiftManager.GetInstance();
        List<string> usernames = new List<string>();
        foreach (string username in glm.m_Players.Values)
        {
            usernames.Add(username);
        }
        usernames.Sort();
        id = usernames.IndexOf(glm.m_Username);

        // if transform != previousTransform then transform has changed locally, so
        //     send transform data in float array to be stored in remoteTransform
        // load remoteTransform into local transform if remoteTransform recieved
        // store transform into previousTransform

        if (transform.localPosition != previousPosition || transform.localRotation != previousRotation || transform.localScale != previousScale)
        {
            // position changed locally
            GetComponent<ASLObject>().SendAndSetClaim(() =>
            {
                float[] f = new float[11];
                f[0] = id;
                f[1] = transform.localPosition.x;
                f[2] = transform.localPosition.y;
                f[3] = transform.localPosition.z;

                f[4] = transform.localRotation.x;
                f[5] = transform.localRotation.y;
                f[6] = transform.localRotation.z;
                f[7] = transform.localRotation.w;

                f[8] = transform.localScale.x;
                f[9] = transform.localScale.y;
                f[10] = transform.localScale.z;

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
