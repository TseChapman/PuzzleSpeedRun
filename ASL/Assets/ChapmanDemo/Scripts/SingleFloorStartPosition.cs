using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFloorStartPosition : MonoBehaviour
{
    public SpawnArea spawnArea;
    private PlayerSystem m_playerSystem;
    public SingleFloorSystem m_floorSystem;

    public void PlaceCharacterInStartPos()
    {
        if (!m_playerSystem.GetIsHost()) return;

        int numCharacterInFloor = m_floorSystem.GetNumCharacterInList();

        if (numCharacterInFloor <= 0)
        {
            Debug.Log("Error: No character added to the floor");
            return;
        }

        for (int i = 0; i < numCharacterInFloor; i++)
        {
            GameObject character = m_floorSystem.GetCharacterByIndex(i);
            Vector3 startPos = spawnArea.GetEmptySpawnPosition();
            character.transform.position = startPos;
            character.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                Debug.Log("Inside Setand Set ");
                character.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(startPos);
            });
        }
    }

    private void Awake()
    {
        m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();
        //m_mazeSystem = GameObject.FindObjectOfType<MazeSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
