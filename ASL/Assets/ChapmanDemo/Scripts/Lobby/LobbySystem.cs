using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbySystem : MonoBehaviour
{
    public SpawnArea spawnArea;
    public TMP_Text selectedLevelText;
    private PlayerSystem m_playerSystem;
    //private Queue<string> m_levels = new Queue<string>();
    private LinkedList<string> m_levels = new LinkedList<string>();
    private LinkedListNode<string> m_currentNode;
    private string m_selectedLevel = "";
    private static int m_isLeftOrRightSignal = 0; // 0 = no change, 1 = previous, 2 = next

    /// <param name="_myFloats">My float 4 array</param>
    /// <param name="_id">The id of the object that called <see cref="ASL.ASLObject.SendFloatArray_Example(float[])"/></param>
    public static void FloatCallback(string _id, float[] _floatArr)
    {
        float value = _floatArr[0];
        m_isLeftOrRightSignal = (int)value;
        if (value > 2f) m_isLeftOrRightSignal = 0;
    }

    private void GetPreviousSelectedLevel()
    {
        if (m_currentNode.Previous == null) return;
        Debug.Log("Previous");
        selectedLevelText.text = m_currentNode.Previous.Value;
        m_currentNode = m_currentNode.Previous;
    }

    private void GetNextSelectedLevel()
    {
        if (m_currentNode.Next == null) return;
        Debug.Log("Next");
        selectedLevelText.text = m_currentNode.Next.Value;
        m_currentNode = m_currentNode.Next;
    }

    public void InitLobby()
    {
        if (!m_playerSystem.GetIsHost()) return;

        int numPlayer = m_playerSystem.GetNumPlayers();
        if (numPlayer <= 0)
        {
            Debug.Log("Error: No character added to the maze");
            return;
        }

        for (int i = 0; i < numPlayer; i++)
        {
            GameObject player = m_playerSystem.GetPlayerByIndex(i);
            Vector3 spawnPos = spawnArea.GetEmptySpawnPosition();
            player.transform.position = spawnPos;
            // Set character position to bottom floor
            player.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                player.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(spawnPos);
            });
        }
    }

    private void DefaultSelectedLevel()
    {
        selectedLevelText.text = m_levels.First.Value;
        m_currentNode = m_levels.First;
    }

    private void InitList()
    {
        m_levels.AddLast("Easy Level");
        m_levels.AddLast("Medium Level");

        DefaultSelectedLevel();
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();
        this.gameObject.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(FloatCallback);

        InitList();
        InitLobby();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_isLeftOrRightSignal == 1)
        {
            GetPreviousSelectedLevel();
            m_isLeftOrRightSignal = 0;
        }
        else if (m_isLeftOrRightSignal == 2)
        {
            GetNextSelectedLevel();
            m_isLeftOrRightSignal = 0;
        }
    }
}
