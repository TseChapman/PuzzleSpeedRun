using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using System.Text;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Teleport;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class VRPlayerMovement : MonoBehaviour{


    public static Transform playerMeshTransform;  //Stores playerMesh  
    private static GameObject m_playerMeshObject;
    private GameObject m_childPlayerMeshObject;
    private PlayerPeerId m_playerPeerId;
    public Vector3 spawnPoint = Vector3.zero;   //Base point of the player spawn point (Player will be spawned randomly within playerRandomSpwanRange from this point)
    public float movementSensitivity = 3f; //Walking sensitivity
    //public float jumpForce = 6f; //Jump functionality closed
    public LayerMask groundLayerMask; //Layer Mask for ground
    public LayerMask playerMeshLayerMask; //Layer Mask for player mesh
    public LayerMask pickAbleItemLayerMask; //Layer Mask for pickable Items
    public LayerMask pickAbleItemChildLayerMask; //Layer Mask for pickable Items
    private float PlayerScale = 1f;
    private bool grounded; //Check if the player is on the ground 
    private bool onObject = false; //check if the player is on top of the pickable object
    private CharacterController controller; //Stores player's Character controller component
    public float gravity = -9.81f;  //Gravity to calculate falling speed
    private float fallingSpeed; //Stores falling speed
    private ASLTransformSync myASL;
    private Vector3 onObjectPos; //Player position when on the top of the pickable object
    PCPlayerItemInteraction pcPlayerItemInteraction;
    public bool isGrabbing { get; set; } = false;
    private bool spawnPointSet = false; //True if player System set its spawn point
    public bool continousMovementOn = true;
    private bool disabledLeftTeleport = false;
    Quaternion cameraRotation;
    private bool isTeleporting = false;
    private XRRig rig;
    public XRNode inputSource;
    public float additionalHeight = 0.2f;
    private Vector2 inputAxis;
    public GameObject vrPresenceObject;
    private VRPresence vrPresence;
    public bool usingASL = true;
    public int[] pickAbleLayerNum;
    private bool m_isPeerIdSet = false; // True if m_playerMeshObject is set with client's peer id

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
        PlayerScale = transform.localScale.y;
        controller.center = new Vector3(0, PlayerScale, 0);
        pcPlayerItemInteraction = GetComponent<PCPlayerItemInteraction>();
        //calculate spawn point
        vrPresence = vrPresenceObject.GetComponent<VRPresence>();
        if (usingASL)
        initPlayerMeshToThePoint();
        
    }

    void Update()
    {
        //if (playerMeshTransform == null)
        //{
         //   tryGettingPlayerMesh();
        //}
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        if (usingASL)
            movePlayerMesh();
        
    }


    private void FixedUpdate()
    {
        if (spawnPointSet)
        {
                characterControllerFollowHeadset();
            getCameraDirection();
            movePlayerMovementbyJoystick();
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
        ASL.ASLHelper.InstantiateASLObject("PlayerMesh", randomInitPoint, Quaternion.identity, null, null, InitCallBack);
    }

    private static void InitCallBack(GameObject _gameobject)
    {
        playerMeshTransform = _gameobject.transform;
        m_playerMeshObject = _gameobject;
    }

    //This will look for any playerMesh initiate and store it to the playerMeshTransform
    /*
    void tryGettingPlayerMesh()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.02f, playerMeshLayerMask);
        //Debug.Log("hitCollider has "  + hitColliders);
        if (hitColliders.Length > 0)
        {
            playerMeshTransform = hitColliders[0].transform;      
        }       
    }
    */

    private void getCameraDirection()
    {
        cameraRotation = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
    }


    //This method will allow the user to move the player with their keyboard
    void movePlayerMovementbyJoystick() {
        Vector3 move = cameraRotation * new Vector3(inputAxis.x, 0, inputAxis.y);
        controller.Move(move * movementSensitivity * Time.fixedDeltaTime);        
    }

    //This method will make player to fall to the ground if the player is not on the ground
    void fallPlayer()
    {
        Vector3 rayStart = transform.TransformPoint(controller.center);
        float rayLength = controller.center.y + 0.01f;
        grounded = Physics.SphereCast(rayStart, controller.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayerMask);
        foreach (int i in pickAbleLayerNum)
        {
            Physics.IgnoreLayerCollision(gameObject.layer, i, isGrabbing);
        }
        if (grounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;
        controller.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
        if (transform.position.y < -99f) //fall too much..
        {
            transform.position = spawnPoint;
        }
    }
    void characterControllerFollowHeadset()
    {
        controller.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        controller.center = new Vector3(capsuleCenter.x, controller.height / 2 + controller.skinWidth , capsuleCenter.z);
    }

  
    //Move the player to where ASL is initialized
    //After ASLTransformSync is applied to ASL object, allow the user to move ASL object. 
    void movePlayerMesh()
    {
        if (playerMeshTransform != null)
        {
            if (!playerMeshTransform.GetComponent<ASLPlayerSync>())
            {
                transform.position = playerMeshTransform.position + new Vector3(0, PlayerScale, 0);
                
            }
            else
            {
                spawnPointSet = true;
                playerMeshTransform.position = new Vector3(rig.cameraGameObject.transform.position.x ,controller.transform.position.y, rig.cameraGameObject.transform.position.z) + new Vector3(0, PlayerScale ,0);
                playerMeshTransform.rotation = cameraRotation;
            }
        }
        if (m_playerMeshObject != null && !m_isPeerIdSet)
        {
            m_childPlayerMeshObject = m_playerMeshObject.transform.GetChild(0).gameObject;
            m_playerPeerId = m_playerMeshObject.transform.GetChild(1).gameObject.GetComponent<PlayerPeerId>();
            int peerId = GameLiftManager.GetInstance().m_PeerId;
            string id = m_playerMeshObject.GetComponent<ASL.ASLObject>().m_Id;
            Debug.Log("Before Send Peer Id to PlayerPeerId: peerid = " + peerId + " obj id = " + id);
            m_playerPeerId.SetPeerId(peerId);
            m_isPeerIdSet = true;
        }

        if (m_childPlayerMeshObject != null)
        {
            PlayerTeleport plyTeleport = m_childPlayerMeshObject.GetComponent<PlayerTeleport>();
            if (plyTeleport != null && plyTeleport.GetIsSignalTeleport())
            {
                Debug.Log("Get Pos from VR");
                Vector3 newPos = plyTeleport.GetPos();
                controller.enabled = false;
                this.gameObject.transform.position = newPos;
                controller.enabled = true;
                plyTeleport.ResetSignal();
            }
        }
    }

}
