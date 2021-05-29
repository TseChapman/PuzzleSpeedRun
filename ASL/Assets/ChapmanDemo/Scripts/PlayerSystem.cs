using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public enum LevelType
{
    SINGLE_FLOOR = 0,
    DOUBLE_FLOOR = 1,
    NUM_LEVEL_TYPE = 2
};

public class PlayerSystem : MonoBehaviour
{
    public string levelPrefabName = "LobbyPrefab";
    public LevelType levelType = LevelType.DOUBLE_FLOOR;
    private bool m_isHost = false;
    private bool m_isInit = false;
    private bool m_isLobbyInit = false;
    private static bool m_isLobbyStored = false;
    private bool m_isLobbyStarted = false;
    private bool m_isPeerIdCallBackSet = false;

    // TODO: Allow teams. Maybe List<List<GameObject>> ?
    private static GameObject lobby;
    private List<GameObject> m_playerList = new List<GameObject>();
    private Dictionary<string, int> m_playerObjDict = new Dictionary<string, int>();
    private int m_playerIndex = 0;

    public bool GetIsPeerIdCallBackSet() { return m_isPeerIdCallBackSet; }

    //TODO: Maybe move it to a level system
    /// <param name="_gameObject">The gameobject that was created</param>
    public static void StoreLobby(GameObject _gameObject)
    {
        //An example of how we can get a handle to our object that we just created but want to use later
        lobby = _gameObject;
        m_isLobbyStored = true;
    }

    /// <param name="_id">The id of the object who's claim was rejected</param>
    /// <param name="_cancelledCallbacks">The amount of claim callbacks that were cancelled</param>
    public static void ClaimRecoveryFunction(string _id, int _cancelledCallbacks)
    {
        Debug.Log("Aw man. My claim got rejected for my object with id: " + _id + " it had " + _cancelledCallbacks + " claim callbacks to execute.");
        //If I can't have this object, no one can. (An example of how to get the object we were unable to claim based on its ID and then perform an action). Obviously,
        //deleting the object wouldn't be very nice to whoever prevented your claim
        if (ASL.ASLHelper.m_ASLObjects.TryGetValue(_id, out ASL.ASLObject _myObject))
        {
            _myObject.GetComponent<ASL.ASLObject>().DeleteObject();
        }

    }

    /// <param name="_id"></param>
    /// <param name="_myFloats"></param>
    public static void MyFloatsFunction(string _id, float[] _myFloats)
    {
        Debug.Log("The floats that were sent are:\n");
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(_myFloats[i] + "\n");
        }
    }

    public void InitializeLobby()
    {
        if (!m_isInit) return;
        if (m_isLobbyInit) return;
        // Instantiate the maze prefab
        if (levelPrefabName == "")
        {
            Debug.Log("Error: PlayerSystem: Empty LevelPrefabName, set to default LobbyPrefab");
            levelPrefabName = "LobbyPrefab";
        }
        ASL.ASLHelper.InstantiateASLObject(levelPrefabName,
                                   new Vector3(0f, 0f, 0f), // TODO: Should have a parameter object
                                   Quaternion.identity, "", "",
                                   StoreLobby,
                                   ClaimRecoveryFunction,
                                   MyFloatsFunction);
        m_isLobbyInit = true;
    }

    /*
    public void StartMaze()
    {
        if (!m_isLobbyStored) return;
        if (m_isLobbyStarted) return;
        lobby.GetComponent<LobbySystem>().InitMaze();
        m_isLobbyStarted = true;
        if (levelType != LevelType.DOUBLE_FLOOR) return;
        if (!m_isMazeStored) return;
        if (m_isMazeStarted) return;
        maze.GetComponent<MazeSystem>().InitMaze();
        m_isMazeStarted = true;
    }
    */
    /*
    public void StartSingleFloor()
    {
        if (levelType != LevelType.SINGLE_FLOOR) return;
        if (!m_isMazeStored) return;
        if (m_isMazeStarted) return;
        maze.GetComponent<SingleFloorSystem>().InitFloor();
        m_isMazeStarted = true;
    }
    */
    public bool GetIsHost() { return m_isHost; }

    public int GetNumPlayers() { return m_playerList.Count; }

    public GameObject GetPlayerByIndex(int index) 
    { 
        if (index < GetNumPlayers())
            return m_playerList[index];
        return null;
    }

    private void Awake()
    {
        // PlayerSystem should not be destroyed
        DontDestroyOnLoad(this.gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    private void InitPlayers()
    {
        // Find all character objects in the scene by tag or level
        if (!m_isHost) return;
        if (m_isInit) return;
        //Debug.Log("I am the Host");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // TODO: Switch to the player script
        //Debug.Log("Num player in PlayerSystem ASL = " + GameLiftManager.GetInstance().m_Players.Count);
        //Debug.Log("Num player in players  = " + players.Length);
        if (players.Length < GameLiftManager.GetInstance().m_Players.Count) return;
        //Debug.Log("Num player in PlayerSystem = " + players.Length);
        foreach (GameObject characterObj in players)
        {
            string id = characterObj.GetComponent<ASL.ASLObject>().m_Id;
            if (!m_playerObjDict.ContainsKey(id))
            {
                m_playerList.Add(characterObj);
                m_playerObjDict.Add(id, m_playerIndex);
                Debug.Log("obj's player index = " + id);
                m_playerIndex++;
            }
            //Debug.Log("Character Object name: " + characterObj.name);
        }
        //Debug.Log("Num player added to the list: " + m_playerList.Count);
        m_isInit = true;
    }

    private void InitPeerIdCallBack()
    {
        if (!m_isInit) return;
        if (m_isPeerIdCallBackSet) return;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in players)
        {
            PlayerPeerId pId = g.transform.GetChild(1).gameObject.GetComponent<PlayerPeerId>();
            Debug.Log("PlayerSystem: InitPeerIdCallBack(): Set callback function");
            pId.SetCallBack();
        }
        m_isPeerIdCallBackSet = true;
    }

    private void TestSetPeerId()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            GameObject g = players[i];
            PlayerPeerId pId = g.transform.GetChild(1).gameObject.GetComponent<PlayerPeerId>();
            Debug.Log("PlayerSystem: TestSetPeerId(): Test Set peer id = " + (100+i));
            pId.SetPeerId(100 + i);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        m_isHost = GameLiftManager.GetInstance().AmLowestPeer();
        InitPlayers();
        InitPeerIdCallBack();
        // Testing
        InitializeLobby();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestSetPeerId();
        }
        return;
        //StartMaze();
        //InitializeMaze();
        //StartSingleFloor();
    }
}
