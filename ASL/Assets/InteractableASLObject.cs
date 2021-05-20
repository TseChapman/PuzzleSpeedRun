using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableASLObject : MonoBehaviour
{
    public bool isInteracting = false;
    public int[] myLayer;
    public int[] colliderLayerToDisable;
    public string startInteractingEventName;
    public string stopInteractingEventName;
    public EventSync myEventSync;
    public Collider[] additionalColliderToDisable; //When interacting
    public bool disableMyColliderWhenInteracting = true;

    public void startInteractingWithObject()
    {
        myEventSync.Activate(startInteractingEventName);
    }

    //This will be called whenever the player stop interatcing with an ASL moveable object
    public void stopInteractingWithObject()
    {
        myEventSync.Activate(stopInteractingEventName);
    }



    //This will be called whenever the player start interatcing with an ASL moveable object
    public void startInteracting()
    {
        if(!isInteracting)
        {
            isInteracting = true;
            disableColliding();
        }
        
    }

    //This will be called whenever the player stop interatcing with an ASL moveable object
    public void stopInteracting()
    {
        if (isInteracting)
        {
            isInteracting = false;
            enableColliding();
        }
        
    }

    public void disableColliding()
    {
        GameObject[] playerRig = GameObject.FindGameObjectsWithTag("PlayerRig");
        foreach (GameObject g in playerRig)
        {
            if(disableMyColliderWhenInteracting)
                Physics.IgnoreCollision(g.GetComponent<Collider>(), GetComponent<Collider>(), true);
            foreach (Collider c in additionalColliderToDisable)
            {
                Physics.IgnoreCollision(g.GetComponent<Collider>(), c, true);
            }             
        }
    } 

    public void enableColliding()
    {
        GameObject[] playerRig = GameObject.FindGameObjectsWithTag("PlayerRig");
        foreach (GameObject g in playerRig)
        {
            if (disableMyColliderWhenInteracting)
                Physics.IgnoreCollision(g.GetComponent<Collider>(), GetComponent<Collider>(), false);
            foreach (Collider c in additionalColliderToDisable)
            {
                Physics.IgnoreCollision(g.GetComponent<Collider>(), c, false);
            }
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (!isInteracting) return;
        foreach (int i in colliderLayerToDisable)
        {
            if (collision.gameObject.layer == i)
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
                break;
            }
        }
    }*/

}
