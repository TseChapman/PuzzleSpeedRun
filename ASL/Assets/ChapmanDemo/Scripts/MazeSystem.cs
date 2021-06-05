using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSystem : MonoBehaviour
{
    // Test/Demo user system
    private PlayerSystem m_playerSystem;

    public MazeStartPosition m_mazeStartPosition;
    public MazeEndPosition m_mazeEndPosition;
    private List<GameObject> m_characterList = new List<GameObject>();
    private List<GameObject> m_characterListOnBottomFloor = new List<GameObject>();
    private List<GameObject> m_characterListOnTopFloor = new List<GameObject>();
    private bool m_isMazeEnded = false;
    private int teamId = -1;

    public void SetTeamId(int id) { teamId = id; }
    public int GetTeamId() { return teamId; }

    /* Functionality for add and accessing character lists */

    // Add character
    public void AddCharacterInMaze(GameObject character) { m_characterList.Add(character); }
    public void AddBottomFloorCharac(GameObject character) { m_characterListOnBottomFloor.Add(character); }
    public void AddTopFloorCharac(GameObject character) { m_characterListOnTopFloor.Add(character); }

    // Number of character in the list
    public int GetNumCharacterInMaze() { return m_characterList.Count; }
    public int GetNumCharacterInBottomFloor() { return m_characterListOnBottomFloor.Count; }
    public int GetNumCharacterInTopFloor() { return m_characterListOnTopFloor.Count; }

    // Accessing GameObject character in the list
    public GameObject GetMazeCharacterByIndex(int index)
    {
        if (index >= m_characterList.Count) return null;
        return m_characterList[index];
    }
    public GameObject GetBottomFloorCharByIndex(int index)
    {
        if (index >= m_characterListOnBottomFloor.Count) return null;
        return m_characterListOnBottomFloor[index];
    }
    public GameObject GetTopFloorCharByIndex(int index)
    {
        if (index >= m_characterListOnTopFloor.Count) return null;
        return m_characterListOnTopFloor[index];
    }

    // Debug Function: Display the List<GameObject>
    public void DisplayList(List<GameObject> list)
    {
        Debug.Log("Num in list = " + list.Count);
        foreach (GameObject obj in list)
        {
            Debug.Log("Obj name = " + obj.name);
        }
    }

    /* Set and Get Function for isMazeEnded */
    public void SetIsMazeEnded(bool isEnded) { m_isMazeEnded = isEnded; }
    public bool GetIsMazeEnded() { return m_isMazeEnded; }

    /* Prublic initialize functions */

    public void StartLevel()
    {
        if (m_playerSystem == null) m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();
        if (!m_playerSystem.GetIsHost()) return;
        if (GetNumCharacterInMaze() == 0) return;
        m_mazeStartPosition.PlaceCharacterInStartPos();
    }

    public void InitMaze()
    {
        if (!m_playerSystem.GetIsHost()) return;
        // Get players
        int numPlayer = m_playerSystem.GetNumPlayers();
        for (int i = 0; i < numPlayer; i++)
        {
            GameObject player = m_playerSystem.GetPlayerByIndex(i);
            if (player != null)
                AddCharacterInMaze(player);
        }

        // Start Maze
        m_mazeStartPosition.PlaceCharacterInStartPos();
    }

    private void InitButtonSystem()
    {
        ASL.ASLHelper.InstantiateASLObject("ButtonSystem",
                                           new Vector3(0f, 0f, 0f),
                                           Quaternion.identity);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Demo system only
        m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();

        //m_mazeStartPosition = GameObject.FindObjectOfType<MazeStartPosition>();
        //m_mazeEndPosition = GameObject.FindObjectOfType<MazeEndPosition>();
        //InitButtonSystem();
    }
}
