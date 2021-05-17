using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFloorSystem : MonoBehaviour
{
    private PlayerSystem m_playerSystem;
    public SingleFloorStartPosition m_singleFloorStartPosition;

    private List<GameObject> m_characterList = new List<GameObject>();

    public void AddCharacterInList(GameObject character) { m_characterList.Add(character); }
    public int GetNumCharacterInList() { return m_characterList.Count; }

    public GameObject GetCharacterByIndex(int index)
    {
        if (index >= m_characterList.Count) return null;
        return m_characterList[index];
    }

    public void InitFloor()
    {
        if (!m_playerSystem.GetIsHost()) return;
        // Get players
        int numPlayer = m_playerSystem.GetNumPlayers();
        for (int i = 0; i < numPlayer; i++)
        {
            GameObject player = m_playerSystem.GetPlayerByIndex(i);
            if (player != null)
                AddCharacterInList(player);
        }

        // Start Maze
        m_singleFloorStartPosition.PlaceCharacterInStartPos();
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
