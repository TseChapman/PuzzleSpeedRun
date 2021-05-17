using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObjectFriction : MonoBehaviour {
    // Start is called before the first frame update
    Rigidbody myRigid;
    Collider myCollider;
    bool isReachedToGround = false;
    bool startFriction = false;
    private void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        StartCoroutine(lockYAfter2Sec());
    }
    IEnumerator lockYAfter2Sec()
    {
        yield return new WaitForSeconds(2);
        //lockY();
        startFriction = true;
    }
    private void Update()
    {
        if(startFriction)
        stopSliding();
    }

    public void lockY()
    {
        myRigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        
    }

    public void unlock()
    {
        myRigid.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    public void unlockY()
    {
        myRigid.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    public void lockRot() //Only allow x,y displacement
    {
        myRigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

    }

    public void unlockRot()
    {
        myRigid.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    public void lockPos() //Only allow y axis rotation
    {
        myRigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX |
            RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

    }

    public void unlockPos()
    {
        myRigid.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }

    private void stopSliding()
    {
        if (!startFriction) return;
        myRigid.velocity = Vector3.zero;
        myRigid.angularVelocity = Vector3.zero;
    }

    public void test()
    {
        Debug.Log("YA!");
    }
}
