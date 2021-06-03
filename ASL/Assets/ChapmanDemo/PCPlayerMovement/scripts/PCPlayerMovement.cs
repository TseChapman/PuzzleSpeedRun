using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using System.Text;
public class PCPlayerMovement : MonoBehaviour
{
    public static Transform playerMeshTransform;
    public static GameObject m_playerMeshObject;
    private GameObject m_childPlayerMeshObject;
    private PlayerPeerId m_playerPeerId;
    private PlayerSystem m_playerSystem;
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
    private Vector3 move;
    public bool usingASL = true;
    private float timer = 2f;

    void Start()
    {
        m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();
        controller = GetComponent<CharacterController>();
        pcPlayerItemInteraction = GetComponent<PCPlayerItemInteraction>();
        //calculate spawn point
        vrPresence = vrPresenceObject.GetComponent<VRPresence>();
        if (!usingASL)
        {
            spawnPointSet = true;
            pcPlayerItemInteraction.notUsingASL();
        }
    }

    void Update()
    {
        if (usingASL)
        {
            movePlayerMesh();
            initPlayerMeshToThePoint();
        }
    }

    private void FixedUpdate()
    {
        if (spawnPointSet)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                movePlayerMovementbyKeyboard();
            }
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
        timer *= (float)GameLiftManager.GetInstance().m_PeerId;
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
        move = transform.right * x + transform.forward * y;
        controller.Move(move * movementSensitivity * Time.fixedDeltaTime);        
    }

    //This method will make player to fall to the ground if the player is not on the ground
    void fallPlayer()
    {
        grounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), .5f, groundLayerMask);
        //onObject = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), .5f, pickAbleItemLayerMask);

        //Debug.Log("Grounded: "+grounded);
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
        if (m_playerMeshObject != null && !m_isPeerIdSet && m_playerSystem != null && m_playerSystem.GetIsPeerIdCallBackSet())
        {
            timer -= Time.smoothDeltaTime;
            if (timer <= 0)
            {
                m_childPlayerMeshObject = m_playerMeshObject.transform.GetChild(0).gameObject;
                m_playerPeerId = m_playerMeshObject.transform.GetChild(1).gameObject.GetComponent<PlayerPeerId>();
                if (!m_playerPeerId.GetIsCallBackSet())
                    m_playerPeerId.SetCallBack();
                int peerId = GameLiftManager.GetInstance().m_PeerId;
                string id = m_playerMeshObject.GetComponent<ASL.ASLObject>().m_Id;
                //Debug.Log("Before Send Peer Id to PlayerPeerId: peerid = " + peerId + " obj id = " + id);
                m_playerPeerId.SetPeerId(peerId);
                m_isPeerIdSet = true;
            }
            
        }
        /*
        else
        {
            if (m_playerMeshObject == null)
            {
                Debug.Log("PlayerMeshObject is null");
            }
            if (m_playerSystem == null)
            {
                Debug.Log("PlayerSystem is Null");
            }
            else
            {
                //Debug.Log("GetIsPeerIdCallBackSet() return: " + m_playerSystem.GetIsPeerIdCallBackSet().ToString());
            }
        }
        */
        if (m_childPlayerMeshObject != null)
        {
            PlayerTeleport plyTeleport = m_childPlayerMeshObject.GetComponent<PlayerTeleport>();
            if (plyTeleport != null && plyTeleport.GetIsSignalTeleport())
            {
                Debug.Log("Get Pos from PC");
                Vector3 newPos = plyTeleport.GetPos();
                controller.enabled = false;
                this.gameObject.transform.position = newPos;
                controller.enabled = true;
                plyTeleport.ResetSignal();
            }
        }

    }


}
