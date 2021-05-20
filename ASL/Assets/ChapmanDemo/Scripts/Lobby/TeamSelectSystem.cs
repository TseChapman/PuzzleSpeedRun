using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class TeamSelectSystem : MonoBehaviour
{
    public GameObject nextButton; // switch to level selection page
    public List<Team> m_teams = new List<Team>();
    [SerializeField] private bool m_isDebugMode = false;
    private PlayerSystem m_playerSystem;
    private int m_currentTeam = -1;
    private const int NUM_TEAMS = 10;
    private static int callbackTeamId = -1;
    private static int callbackPeerId = -1;
    private static int callbackActionId = -1;
    private static int callbackPrevTeamId = -1;

    /// <param name="_myFloats">My float 4 array</param>
    /// <param name="_id">The id of the object that called <see cref="ASL.ASLObject.SendFloatArray_Example(float[])"/></param>
    public static void FloatCallback(string _id, float[] _floatArr)
    {
        callbackTeamId = (int)_floatArr[0];
        callbackPeerId = (int)_floatArr[1];
        callbackActionId = (int)_floatArr[2];
        callbackPrevTeamId = (int)_floatArr[3];
        // [0] = team id, [1] = peer id, [2] = action (0 for add, 1 for remove), [3] = previous team id
    }


    // Start is called before the first frame update
    private void Start()
    {
        m_playerSystem = GameObject.FindObjectOfType<PlayerSystem>();
        this.gameObject.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(FloatCallback);
    }

    private void CheckClicks()
    {
        // Check what object does the player clicked on
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 20f))
        {
            GameObject parent = hit.collider.gameObject.transform.parent.gameObject;
            if (parent.tag == "Team") // Check if clicked on a team object
            {
                //Debug.Log("Parent name = " + parent.name);
                for (int i = 0; i < NUM_TEAMS; i++)
                {
                    string teamName = "Team" + (i+1);
                    //Debug.Log("Traverse team name = " + teamName);
                    if (parent.name == teamName)
                    {
                        //Debug.Log("Add member to " + teamName);
                        // Synchronize the information in TeamSelectSystem
                        // float arr: [0] = team id, [1] = peer id
                        int peerId = GameLiftManager.GetInstance().m_PeerId;
                        int previousTeam = m_currentTeam;
                        float[] flt = new float[4];
                        flt[0] = (float)i;
                        flt[1] = (float)peerId;
                        flt[2] = (m_currentTeam != -1) ? 1f : 0;
                        flt[3] = previousTeam;
                        Debug.Log("Add member to " + teamName);
                        m_currentTeam = i;
                        this.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                        {
                            this.gameObject.GetComponent<ASL.ASLObject>().SendFloatArray(flt);
                        });
                    }
                }
            }
            else if (hit.collider.gameObject == nextButton)
            {
                if (isValidTeams())
                {
                    // Changes the page for all client
                }
            }
        }
    }

    private void PerformAction()
    {
        if (callbackActionId == 1)
            m_teams[callbackPrevTeamId].RemoveMember(callbackPeerId);
        m_teams[callbackTeamId].AddMember(callbackPeerId);
    }

    private void ResetAction()
    {
        callbackActionId = -1;
        callbackTeamId = -1;
        callbackPeerId = -1;
        callbackPrevTeamId = -1;
    }

    private bool isValidTeams()
    {
        bool result = true;
        foreach (Team t in m_teams)
        {
            if (t.GetNumMember() < 2)
                result = false;
        }
        return result;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckClicks();
        }
        if (callbackActionId != -1)
        {
            PerformAction();
            ResetAction();
        }
    }
}
