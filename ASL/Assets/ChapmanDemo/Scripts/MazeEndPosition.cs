using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEndPosition : MonoBehaviour
{
    public MovingPlatformPoint Point;
    public GameObject MazeExitDoor;
    private PlayerSystem m_playerSystem;
    public MazeSystem m_mazeSystem;
    [SerializeField] private float m_endMazeDistance = 1f;

    private void Awake()
    {
        m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();
        //m_mazeSystem = GameObject.FindObjectOfType<MazeSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (!m_playerSystem.GetIsHost()) return;
        if (m_mazeSystem.GetIsMazeEnded()) { return; }
        
        int numPlayerInBottomFloor = m_mazeSystem.GetNumCharacterInBottomFloor();

        if (numPlayerInBottomFloor <= 0) { return; }

        bool isAnyCharacNotEnded = false;
        for (int i = 0; i < numPlayerInBottomFloor; i++)
        {
            GameObject character = m_mazeSystem.GetBottomFloorCharByIndex(i);
            //Debug.Log("Character name: " + character.name); // Test if character is added
            float dist = Vector3.Distance(character.transform.position, this.gameObject.transform.position);
            //Debug.Log("Character : " + character.name + " distance to end position is " + dist);
            if (dist > m_endMazeDistance)
            {
                Debug.Log("Some character has not pass the maze");
                isAnyCharacNotEnded = true;
            }
        }

        if (isAnyCharacNotEnded is false)
        {
            Debug.Log("Maze passed");
            m_mazeSystem.SetIsMazeEnded(true);
            Point.SetSpeed = true;
            Point.Speed = 4f;
            MazeExitDoor.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                MazeExitDoor.GetComponent<ASL.ASLObject>().DeleteObject();
            });
        }
    }
}
