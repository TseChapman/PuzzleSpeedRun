using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class isPicked : MonoBehaviour
{
    private float localFloat;
    private float remoteFloat;
    private float previousFloat;
    public bool isInteracting = false;
    private int id;
    private bool madeInitialClaim;
    private bool floatChangedRemotely;
    private int lockHolder;
    public bool testInteractiveOn = false;
    public bool testInteractiveInteracting = false;
    void Start()
    {
        GetComponent<ASLObject>()._LocallySetFloatCallback((string _id, float[] f) =>
        {
            //lockHolder = id;
            Debug.Log("CUBE REMOTE CHANGE? CallBack");
            if (id != f[0])
            {
                remoteFloat = f[1];
                Debug.Log("CUBE REMOTE CHANGE? CallBack set remote float");
                floatChangedRemotely = true;
            }
        });
    }
    public void pickUp()
    {
        localFloat = 1;
    }

    public void release()
    {
        localFloat = 0;
    }

    public bool isPickedUp()  
    {
        return isInteracting;
    }
    private void updateBoolBasedOnFloat()
    {
        if (localFloat == 0)
        {
            isInteracting = false;
        }
            
        else
        {
            isInteracting = true;
        }
           
        
    }
    private void testInteractiveOnTEST()
    {
        if(testInteractiveOn)
        {
            if (testInteractiveInteracting)
            {
                remoteFloat = 1;
                Debug.Log("isPickedScript. other person start intereacting ");
                floatChangedRemotely = true;
            } else
            {
                remoteFloat = 0;
                Debug.Log("isPickedScript. other person stopped intereacting ");
                floatChangedRemotely = true;
            }




            testInteractiveOn = false;
        }
        
    }
    private void Update()
    {
        testInteractiveOnTEST();
        GameLiftManager glm = GameLiftManager.GetInstance();
        if (!glm)
        {
            Debug.Log("NOT GETTING GLM");
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
        //Debug.Log("SEND?1");
        if (localFloat != previousFloat)
        {
            //Debug.Log("SEND?2");
            GetComponent<ASLObject>().SendAndSetClaim(() =>
            {
                float[] f = new float[2];
                f[0] = id;
                f[1] = localFloat;
                //Debug.Log("SEND?3" + localFloat);
                GetComponent<ASLObject>().SendFloatArray(f);
            });
        }
        
        if (floatChangedRemotely)
        {
            Debug.Log("CUBE REMOTE CHANGE?");
            localFloat = remoteFloat;
            floatChangedRemotely = false;
            
        }
        updateBoolBasedOnFloat();
        previousFloat = localFloat;
        //checkAndChangeForTest();
    }

    void checkAndChangeForTest()
    {
        if(isInteracting)
        {
            transform.parent.GetComponent<Renderer>().material.color = Color.blue;
        } else
        {
            transform.parent.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    /*
        // if transform != previousTransform then transform has changed locally, so
        //     send transform data in float array to be stored in remoteTransform
        // load remoteTransform into local transform if remoteTransform recieved
        // store transform into previousTransform

        if (transform.position != previousPosition || transform.localRotation != previousRotation || transform.localScale != previousScale)
        {
            // position changed locally
            GetComponent<ASLObject>().SendAndSetClaim(() =>
            {
                float[] f = new float[11];
                f[0] = id;
                f[1] = transform.position.x;
                f[2] = transform.position.y;
                f[3] = transform.position.z;

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
            transform.position = remotePosition;
            transform.localRotation = remoteRotation;
            transform.localScale = remoteScale;
            transformChangedRemotely = false;
        }
        previousPosition = transform.position;
        previousRotation = transform.localRotation;
        previousScale = transform.localScale;*/

}
