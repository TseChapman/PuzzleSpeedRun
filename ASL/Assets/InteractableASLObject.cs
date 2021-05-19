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
        /*
        foreach (int i in colliderLayerToDisable)
        {
            foreach (int j in myLayer)
            {
                Debug.Log("DISABLE BETWEEN " + i + " " + j);
                Physics.IgnoreLayerCollision(i, j, true);
            }
        }*/
        GameObject[] playerRig = GameObject.FindGameObjectsWithTag("PlayerRig");
        foreach (GameObject g in playerRig)
        {
            Physics.IgnoreCollision(g.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
    } 

    public void enableColliding()
    {
        /*
            foreach (int i in colliderLayerToDisable)
            {
                foreach (int j in myLayer)
                {
                    Debug.Log("Enable BETWEEN " + i + " " + j);
                    Physics.IgnoreLayerCollision(i, j, false);
                }
            }*/
        GameObject[] playerRig = GameObject.FindGameObjectsWithTag("PlayerRig");
        foreach (GameObject g in playerRig)
        {
            Physics.IgnoreCollision(g.GetComponent<Collider>(), GetComponent<Collider>(), false);
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
