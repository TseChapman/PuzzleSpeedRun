using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerItemInteraction : MonoBehaviour {

    public GameObject pickedUpItem; //Store the item that user is currently picking up
    public GameObject pushingItem;
    private GameObject mirror;
    public float pickUpDistance = 4f; //Maximum distance that allow user to pick up
    public float distanceBetweenPlayerAndObject = 3f;
    public float throwingYDirection = 0.3f; //y direction for parabola projectile angle
    public float throwingForce = 200f;  //throwing force for parabola projectile 
    public LayerMask pickableLayer; //Layer Mask for pickable items layer
    public LayerMask pickableChildLayer; //Layer Mask for pickable items layer
    public LayerMask pushAbleLayer; //Layer Mask for pickable items layer
    public LayerMask mirrorLayer; 
    public LayerMask nonInterativeLayer; //TODO pick a better name?
    private PCPlayerMovement pCPlayerMovement;
    public float mouseSensitivity; // sensitivity for rotation of picked-up objects
    private Vector3 prevPlayerPos;
    private float initMovementSensitivity;
    public float pushingMovementSensitivity = 1f;
    private float pickUpObjectDistance = 3f; //Distance between the player's eye and picked up item
                                             // Update is called once per frame
    private Quaternion initRotation;
    private Quaternion mirrorInitRotation;
    private bool pushableItemClicked;
    private bool mirrorClicked;
    private bool usingASL = true;
    private void Start()
    {
        pCPlayerMovement = GetComponent<PCPlayerMovement>();
        initMovementSensitivity = pCPlayerMovement.movementSensitivity;
    }
    private void FixedUpdate()
    {
        if (pushingItem)
        {
            pushPushableObject();
        }
        if (mirrorClicked)
        {
            
            rotateMirrorMirror();
        }
    }

    public void notUsingASL()
    {
        usingASL = false;
    }

    void pushPushableObject()
    {
        Vector3 move = (transform.position - prevPlayerPos);
        pushingItem.transform.position = pushingItem.transform.position + move;
        prevPlayerPos = transform.position;
        Vector2 mouseOffset = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        pushingItem.transform.RotateAround(pushingItem.transform.position,  Vector3.up, -5 * mouseOffset.x);
    }
    void rotateMirrorMirror()
    {
        Debug.Log("Mirror Update");
        Vector2 mouseOffset = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mirror.transform.localRotation *= Quaternion.AngleAxis(10 * mouseOffset.y, Vector3.right);
    }

    void Update()
    {
        if (!pushingItem)
        {
            pCPlayerMovement.movementSensitivity = initMovementSensitivity;
        }
        if (Input.GetKeyDown("e"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpObjectDistance))
            {
                OnAction onAction = hit.collider.GetComponent<OnAction>();
                if (onAction != null)
                {
                    onAction.OnUse.Invoke();
                }
            }
        }

        if (Input.GetKeyUp("e"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpObjectDistance))
            {
                OnAction onAction = hit.collider.GetComponent<OnAction>();
                if (onAction != null)
                {
                    onAction.OnUseUp.Invoke();
                }
            }
        }

        pickUpObjectDistance = distanceBetweenPlayerAndObject;
        //if (pickedUpItem && collidingObject)
        if (pickedUpItem)
        {
            RaycastHit hitInfo;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 5f, ~pickableLayer & ~pickableChildLayer & ~nonInterativeLayer))
            {
                //Debug.Log(hitInfo.collider.gameObject.name);
                pickUpObjectDistance = hitInfo.distance - .3f;
                if (pickUpObjectDistance < 0)
                    pickUpObjectDistance = 0;
                if (pickUpObjectDistance > distanceBetweenPlayerAndObject)
                    pickUpObjectDistance = distanceBetweenPlayerAndObject;
            }
        }
        

        //pick up item
        if (Input.GetMouseButtonDown(0) )
        {   if (pickedUpItem == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpDistance, pickableLayer))
                {
                    Debug.Log("Did Hit " + hit.transform.name);
                    if (usingASL) {
                        if (!hit.collider.gameObject.transform.GetComponent<InteractableASLObject>().isInteracting)
                        {
                            pickedUpItem = hit.collider.gameObject;
                            pickedUpItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                        }
                    } else
                    {
                        pickedUpItem = hit.collider.gameObject;
                        pickedUpItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    }              
                }
                /*else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpDistance, pickableChildLayer))
                {
                    Transform t = hit.transform.parent;
                    while (t != null)
                    {
                        if ((1 << t.gameObject.layer) == pickableLayer.value)
                        {
                            Debug.Log("Did Hit " + t.name);
                            if (!t.gameObject.transform.GetChild(0).GetComponent<isPicked>().isPickedUp())
                            {
                                pickedUpItem = t.gameObject;
                                pickedUpItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                                break;
                            }
                        }
                        t = t.parent;
                    }
                }*/
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpDistance, pushAbleLayer))
                {
                    pushableItemClicked = true;
                }
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpDistance, mirrorLayer))
                {
                    mirrorClicked = true;

                    Debug.Log("Mirror Clicked!");
                    mirrorInitRotation = hit.collider.gameObject.transform.rotation;
                    mirror = hit.collider.gameObject;
                }
                if (usingASL && pickedUpItem) pickedUpItem.gameObject.transform.GetComponent<InteractableASLObject>().startInteractingWithObject();
            } else {
                leaveObejct();
            }


        }
        if (Input.GetMouseButton(0))
        {
            if (mirrorClicked)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpDistance, mirrorLayer))
                {
                }
                else
                {
                    pushingItem = null;
                    mirrorClicked = false;
                }
            }
            
           if(pushableItemClicked)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpDistance, pushAbleLayer))
                {
                    if (!pushingItem)
                    {
                        prevPlayerPos = transform.position;
                        initRotation = hit.collider.gameObject.transform.rotation;
                        pCPlayerMovement.movementSensitivity = pushingMovementSensitivity;
                    }
                    pushingItem = hit.collider.gameObject;
                    //startInteracting

                }
                else
                {
                    pushingItem = null;
                    pushableItemClicked = false;
                }
            } 
           
        }
        else
        {
            pushingItem = null;
            pushableItemClicked = false;
            mirrorClicked = false;
        }

        //throw item
        if (Input.GetMouseButtonDown(1))
        {
            if (pickedUpItem != null)
            {
                GameObject objectToThrow = pickedUpItem;
                leaveObejct();
                Debug.Log("Throw!");
                objectToThrow.GetComponent<Rigidbody>().AddForce((Camera.main.transform.forward + new Vector3(0, throwingYDirection, 0))* throwingForce);
            }
        }

        //rotate item
        if (Input.GetKeyDown("q"))
        {
            Camera.main.GetComponent<MouseFirstPersonView>().enabled = false;
        }
        if (Input.GetKeyUp("q"))
        {
            Camera.main.GetComponent<MouseFirstPersonView>().enabled = true;
        }
        if (pickedUpItem != null && Input.GetKey("q"))
        {
            float mouseX = Input.GetAxis("Mouse X") * -1 * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            pickedUpItem.transform.Rotate(transform.up, mouseX, Space.World);
            pickedUpItem.transform.Rotate(transform.right, mouseY, Space.World);
        }

        //keep the picked item at the center
        if (pickedUpItem != null)
        {
            Vector3 cameraDirection = Camera.main.transform.forward;
            pickedUpItem.transform.position = Camera.main.transform.position + cameraDirection * pickUpObjectDistance;
        }
    }
    
    private void leaveObejct()
    {
        pickedUpItem.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        pickedUpItem.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (usingASL)
            pickedUpItem.gameObject.transform.GetComponent<InteractableASLObject>().stopInteractingWithObject();
        pickedUpItem = null;
    }
    
}
