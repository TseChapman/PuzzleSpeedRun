using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using TMPro;

public class LobbyStartButton : MonoBehaviour
{
    public TMP_Text teamReadyText;
    public TMP_Text levelReadyText;
    public LobbySystem lobbySystem;
    public TeamSelectSystem teamSelectSystem;
    private PlayerSystem m_playerSystem;
    private static List<GameObject> m_levelPrefabs = new List<GameObject>();
    private static int numTeam = -1;
    private bool m_isPrefabCreated = false;
    private bool isTeamSet = false;
    private bool m_isPlayerAdded = false;
    private float timer = 1f;

    public void ResetLobbyButton()
    {
        m_levelPrefabs.Clear();
        numTeam = -1;
        m_isPrefabCreated = false;
        isTeamSet = false;
        m_isPlayerAdded = false;
        timer = 1f;
    }

    /// <param name="_gameObject">The gameobject that was created</param>
    public static void StoreLevel(GameObject _gameObject)
    {
        //An example of how we can get a handle to our object that we just created but want to use later
        m_levelPrefabs.Add(_gameObject);
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

    /// <param name="_myFloats">My float 4 array</param>
    /// <param name="_id">The id of the object that called <see cref="ASL.ASLObject.SendFloatArray_Example(float[])"/></param>
    public static void FloatCallback(string _id, float[] _floatArr)
    {
        float value = _floatArr[0];
        numTeam = (int)value;
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();
        this.gameObject.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(FloatCallback);
    }

    private bool CheckReady()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 20f))
        {
            if (hit.collider.gameObject.tag == "LobbyStartButton")
            {
                return (teamReadyText.color == Color.green && levelReadyText.color == Color.green);
            }
        }
        return false;
    }

    private void CreatePrefab(int numPrefab)
    {
        string levelName = lobbySystem.GetCurrentLevelName();
        for (int i = 0; i < numPrefab; i++)
        {
            float x_pos = 100f + (40f * i);
            ASL.ASLHelper.InstantiateASLObject(levelName,
                                   new Vector3(x_pos, 0f, 0f), // TODO: Should have a parameter object
                                   Quaternion.identity, "", "",
                                   StoreLevel,
                                   ClaimRecoveryFunction,
                                   MyFloatsFunction);
        }
    }

    private void StartGame()
    {
        // For each team that has 2-4 players, instantiate a level prefab
        // Assign team member peer id to that prefab
        // Use callback to tell all client the game will start and ...
        int numTeam = teamSelectSystem.GetNumTeam();
        if (numTeam < 1)
        {
            Debug.Log("Num team = 0");
            return;
        }
        float[] flr = new float[1];
        flr[0] = (float)numTeam;
        this.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.gameObject.GetComponent<ASL.ASLObject>().SendFloatArray(flr);
        });
    }

    private void SetPrefabTeamId()
    {
        for (int i = 0; i < m_levelPrefabs.Count; i++)
        {
            int teamId = teamSelectSystem.GetTeamIdByIndex(i);
            Debug.Log("Set Prefab Team id = " + teamId);
            m_levelPrefabs[i].GetComponent<MazeSystem>().SetTeamId(teamId);
        }
    }

    private void StartLevel()
    {
        // Add player to the level system
        // Loop through the level prefabs
        int numPlayer = m_playerSystem.GetNumPlayers(); // Number of player
        for (int i = 0; i < m_levelPrefabs.Count; i++)
        {
            // Get the level prefab by index
            MazeSystem mazeSys = m_levelPrefabs[i].GetComponent<MazeSystem>();
            int teamId = mazeSys.GetTeamId(); // Get the teamId for that level prefab
            Debug.Log("StartLevel Team id = " + teamId);
            Team t = teamSelectSystem.GetTeamById(teamId);
            if (t != null)
            {
                // Check the member id and player peer id per team
                int numMem = t.GetNumMember(); // Number of member in that team
                for (int j = 0; j < numMem; j++)
                {
                    int memPeerId = t.GetMemberId(j); // Get member's peer id in the team
                    Debug.Log("StartLevel(): memPeer id = " + GameLiftManager.GetInstance().m_Players[memPeerId]);
                    // Traverse the players and find the player that have the same peer id
                    for (int k = 0; k < numPlayer; k++)
                    {
                        GameObject player = m_playerSystem.GetPlayerByIndex(k); // Get player object
                        PlayerPeerId playerId = player.transform.GetChild(1).gameObject.GetComponent<PlayerPeerId>(); // Get PlayerPeerId
                        int peerId = playerId.GetPeerId();
                        Debug.Log("StartLevel(): Player Id = " + GameLiftManager.GetInstance().m_Players[peerId]);
                        if (peerId != -1 && peerId == memPeerId) // Check if the peerId matches member's peerId
                        {
                            Debug.Log("Add to maze: Peer id = " + GameLiftManager.GetInstance().m_Players[peerId]);
                            mazeSys.AddCharacterInMaze(player); // Add the player to the level
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Team Id is not in teamSelectSystem");
                return;
            }

            mazeSys.StartLevel();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool isReady = CheckReady();
            if (isReady)
            {
                StartGame();
            }
        }
        // Create the prefabs based on number of teams
        if (m_playerSystem.GetIsHost() && numTeam != -1 && !m_isPrefabCreated)
        {
            CreatePrefab(numTeam);
            m_isPrefabCreated = true;
        }
        // If the prefabs are instantiated and team is not set to the prefab, set the team id
        if (m_playerSystem.GetIsHost() && m_levelPrefabs.Count == numTeam && !isTeamSet)
        {
            SetPrefabTeamId();
            isTeamSet = true;
        }
        // If team id is set, start placing player into level's MazeSystem
        if (m_playerSystem.GetIsHost() && isTeamSet && !m_isPlayerAdded)
        {
            timer -= Time.smoothDeltaTime;
            if (timer <= 0)
            {
                // Add player gameObject to MazeSystem
                StartLevel();
                m_isPlayerAdded = true;
            }
            
        }
    }
}
