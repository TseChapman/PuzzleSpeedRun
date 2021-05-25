using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using System.Text;
public class PCPlayerMovement : MonoBehaviour
{
    private static Transform playerMeshTransform;
    private static GameObject m_playerMeshObject;
    public Vector3 spawnPoint = Vector3.zero;   //Base point of the player spawn point (Player will be spawned randomly within playerRandomSpwanRange from this point)
    public float movementSensitivity = 10f; //Walking sensitivity
    //public float jumpForce = 6f; //Jump functionality closed
    public LayerMask groundLayerMask; //Layer Mask for ground
    public LayerMask playerMeshLayerMask; //Layer Mask for player mesh
    public LayerMask pickAbleItemLayerMask; //Layer Mask for pickable Items
    public LayerMask pickAbleItemChildLayerMask; //Layer Mask for pickable Items
    private bool grounded; //Check if the player is on the ground 
    private bool onObject = false; //check if the player is on top of the pickable object
    private CharacterController controller; //Stores player's Character controller component
    public float gravity = -9.81f;  //Gravity to calculate falling speed
    private float fallingSpeed; //Stores falling speed
    private ASLTransformSync myASL;
    private Vector3 onObjectPos; //Player position when on the top of the pickable object
    PCPlayerItemInteraction pcPlayerItemInteraction;
    public GameObject vrPresenceObject;
    private VRPresence vrPresence;
    public int[] pickAbleLayerNum;
    private bool spawnPointSet = false; //True if player System set its spawn point
    private bool m_isPeerIdSet = false; // True if m_playerMeshObject is set with client's peer id

    void Start()
    {
        controller = GetComponent<CharacterController>();
        pcPlayerItemInteraction = GetComponent<PCPlayerItemInteraction>();
        //calculate spawn point
        vrPresence = vrPresenceObject.GetComponent<VRPresence>();

    }

    void Update()
    {
        movePlayerMesh();
        initPlayerMeshToThePoint();
    }

    private void FixedUpdate()
    {
        if (spawnPointSet)
        {
            movePlayerMovementbyKeyboard();
            fallPlayer();
        }
    }

    //This method move the player to the spawn point and instatiate playerMesh
    void initPlayerMeshToThePoint()
    {
        if (!vrPresence.VRorPC) return;
        Vector3 randomInitPoint = new Vector3(Random.Range(-50, 50), Random.Range(-50,50), Random.Range(-50, 50));
        controller.Move(randomInitPoint);
        //playerMesh = Instantiate(PlayerMeshPrefab, randomInitPoint, Quaternion.identity);
        Debug.Log("PLAYER MESH INIT1");
        ASL.ASLHelper.InstantiateASLObject("PlayerMesh", randomInitPoint, Quaternion.identity, null, null, InitCallBack);
        vrPresence.VRorPC = false;
    }
    
    private static void InitCallBack(GameObject _gameobject)
    {
        Debug.Log("PLAYER MESH INIT2");
        playerMeshTransform = _gameobject.transform;
        m_playerMeshObject = _gameobject;
    }
  
    //This will look for any playerMesh initiate and store it to the playerMeshTransform
    void tryGettingPlayerMesh()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.02f, playerMeshLayerMask);
        //Debug.Log("hitCollider has "  + hitColliders);
        if (hitColliders.Length > 0)
        {
            playerMeshTransform = hitColliders[0].transform;      
        }       
    }



    //This method will allow the user to move the player with their keyboard
    void movePlayerMovementbyKeyboard() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        /*rigid body movement system was changed to CharacterController system
        Vector3 movePos = transform.right * x + transform.forward * y;
        Vector3 newMovePos = new Vector3(movePos.x, playerBody.velocity.y, movePos.z);
        playerBody.velocity = newMovePos;
        transform.position = playerBody.position;*/
        Vector3 move = transform.right * x + transform.forward * y;
        controller.Move(move * movementSensitivity * Time.fixedDeltaTime);        
    }

    //This method will make player to fall to the ground if the player is not on the ground
    void fallPlayer()
    {
        grounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), .5f, groundLayerMask);
        onObject = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), .5f, pickAbleItemLayerMask);

        foreach(int i in pickAbleLayerNum)
        {
            Physics.IgnoreLayerCollision(gameObject.layer, i, pcPlayerItemInteraction.pickedUpItem != null);
        }
       
        if (grounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;
        controller.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
        if (transform.position.y < -99f)
        {
            transform.position = spawnPoint;
        }
    }
    // Update is called once per frame

  
    //Move the player to where ASL is initialized
    //After ASLTransformSync is applied to ASL object, allow the user to move ASL object. 
    void movePlayerMesh()
    {
        if (playerMeshTransform != null)
        {
            if (!playerMeshTransform.GetComponent<ASLPlayerSync>())
            {
                transform.position = playerMeshTransform.position + new Vector3(0,2,0);
            }
            else
            {
                spawnPointSet = true;
                playerMeshTransform.position = transform.position;
                playerMeshTransform.rotation = transform.rotation;
            }
        }
        if (m_playerMeshObject != null && !m_isPeerIdSet)
        {
            int peerId = GameLiftManager.GetInstance().m_PeerId;
            float[] flr = new float[1];
            flr[0] = (float)peerId;
            m_playerMeshObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                m_playerMeshObject.GetComponent<ASL.ASLObject>().SendFloatArray(flr);
            });
            m_isPeerIdSet = true;
        }
    }


}
