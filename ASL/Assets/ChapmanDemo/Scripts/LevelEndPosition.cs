using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ASL;

public class LevelEndPosition : MonoBehaviour
{
    private LobbySystem m_lobbySystem;
    private PlayerSystem m_playerSystem;
    public MazeSystem m_mazeSystem;
    public float endLevelDistance = 1f;
    private bool m_isLevelEnded = false;
    private bool m_isPlayerTeleportBack = false;
    public float DebugDelayTimer = 3f;

    public UnityEvent OnWin;

    // Start is called before the first frame update
    private void Start()
    {
        m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();
        m_lobbySystem = GameObject.FindObjectOfType<LobbySystem>();
        GetComponent<ASLObject>()._LocallySetFloatCallback((string id, float[] data) =>
        {
            OnWin.Invoke();
        });
    }

    private void CheckDistance()
    {
        if (!m_playerSystem.GetIsHost()) return;
        if (m_isLevelEnded is true) { return; }
        

        int numPlayerInLevel = m_mazeSystem.GetNumCharacterInMaze();

        if (numPlayerInLevel <= 0) { return; }
        if (m_playerSystem.GetIsDebugMode())
        {
            DebugDelayTimer -= Time.smoothDeltaTime;
            if (DebugDelayTimer < 0)
                m_isLevelEnded = true;
            return;
        }
        bool isAnyCharacNotEnded = false;
        for (int i = 0; i < numPlayerInLevel; i++)
        {
            GameObject character = m_mazeSystem.GetMazeCharacterByIndex(i);
            //Debug.Log("Character name: " + character.name); // Test if character is added
            float dist = Vector3.Distance(character.transform.position, this.gameObject.transform.position);
            //Debug.Log("Character : " + character.name + " distance to end position is " + dist);
            if (dist > endLevelDistance)
            {
                //Debug.Log("Some character has not pass the level");
                isAnyCharacNotEnded = true;
            }
        }

        if (isAnyCharacNotEnded is false)
        {
            Debug.Log("Level passed");
            m_isLevelEnded = true;
            GetComponent<ASLObject>().SendAndSetClaim(() =>
            {
                GetComponent<ASLObject>().SendFloatArray(new float[1]);
            });
        }
    }

    private void CheckIsLevelEnded()
    {
        if (!m_playerSystem.GetIsHost()) return;
        if (m_isLevelEnded is false) { return; }
        if (m_isPlayerTeleportBack is true) { return; }
        // Teleport the players in this maze system back to the lobby:
        if (m_lobbySystem == null)
            m_lobbySystem = GameObject.FindObjectOfType<LobbySystem>();
        // 1.  Reset LobbySystem spawn area empty position index
        m_lobbySystem.ResetLobbySpawn();

        // 2. For each player mesh in the maze system, teleport them back to the lobby scene
        int numPlayerInLevel = m_mazeSystem.GetNumCharacterInMaze();

        if (numPlayerInLevel <= 0) { return; }

        for (int i = 0; i < numPlayerInLevel; i++)
        {
            GameObject character = m_mazeSystem.GetMazeCharacterByIndex(i);
            Debug.Log("Player at index " + i + " teleport back to the lobby");
            m_lobbySystem.ReturnToLobby(character);
        }


        // Destroy the level
        LobbyStartButton button = GameObject.FindObjectOfType<LobbyStartButton>();
        button.DestroyPrefab(m_mazeSystem.GetTeamId());
        m_isPlayerTeleportBack = true;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckDistance();
        CheckIsLevelEnded();
    }
}
