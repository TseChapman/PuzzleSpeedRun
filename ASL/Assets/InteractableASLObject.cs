using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableASLObject : MonoBehaviour
{
    public bool isInteracting = false;
    public int[] myLayer;
    public int[] colliderLayerToDisable;

    //This will be called whenever the player start interatcing with an ASL moveable object
    public void startInteracting()
    {
        if(!isInteracting)
        {
            isInteracting = true;
        }
        disableColliding();
    }

    //This will be called whenever the player stop interatcing with an ASL moveable object
    public void stopInteracting()
    {
        if (isInteracting)
        {
            isInteracting = false;
        }
        enableColliding();
    }

    public void disableColliding()
    {
        foreach (int i in colliderLayerToDisable)
        {
            foreach (int j in myLayer)
            {
                Physics.IgnoreLayerCollision(i, j, true);
            }
        }
    } 

    public void enableColliding()
    {
            foreach (int i in colliderLayerToDisable)
            {
                foreach (int j in myLayer)
                {
                    Physics.IgnoreLayerCollision(i, j, false);
                }
            }
        }

}
