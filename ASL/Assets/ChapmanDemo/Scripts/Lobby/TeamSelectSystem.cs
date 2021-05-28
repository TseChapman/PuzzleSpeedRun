using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using TMPro;

public class TeamSelectSystem : MonoBehaviour
{
    public TMP_Text readyText; // switch to level selection page
    public List<Team> m_teams = new List<Team>();
    private List<int> m_validTeamId = new List<int>();
    public int numMemberPerTeam = 4;
    [SerializeField] private bool m_isDebugMode = false;
    private PlayerSystem m_playerSystem;
    private int m_currentTeam = -1;
    private const int NUM_TEAMS = 10;
    private static int callbackTeamId = -1;
    private static int callbackPeerId = -1;
    private static int callbackActionId = -1;
    private static int callbackPrevTeamId = -1;

    public int GetTeamIdByIndex(int index)
    {
        if (m_validTeamId.Count > index)
            return m_validTeamId[index];
        return -1;
    }

    public int GetNumTeam()
    {
        int numTeam = 0;
        m_validTeamId.Clear();
        foreach (Team t in m_teams)
        {
            if (t.GetNumMember() >= 2 && t.GetNumMember() <= 4)
            {
                m_validTeamId.Add(t.GetTeamId());
                numTeam++;
            }  
        }
        return numTeam;
    }

    public Team GetTeamById(int id)
    {
        foreach (Team t in m_teams)
        {
            if (t.GetTeamId() == id)
            {
                return t;
            }
        }
        return null;
    }

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
        readyText.color = Color.red;
    }

    private void CheckClicks()
    {
        // Check what object does the player clicked on
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 20f))
        {
            if (hit.collider.gameObject.transform.parent == null) return;
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
                        if (!isJoinableTeams(i)) return;
                        //Debug.Log("Add member to " + teamName);
                        // Synchronize the information in TeamSelectSystem
                        // float arr: [0] = team id, [1] = peer id
                        int peerId = GameLiftManager.GetInstance().m_PeerId;
                        int previousTeam = m_currentTeam;
                        float[] flt = new float[4];
                        flt[0] = (float)i; // team id
                        flt[1] = (float)peerId; // player id
                        flt[2] = (m_currentTeam != -1) ? 1f : 0; // action (add or (remove then add))
                        flt[3] = previousTeam; // previous team id, use on action (remove then add)
                        Debug.Log("Add member to " + teamName);
                        m_currentTeam = i;
                        this.gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                        {
                            this.gameObject.GetComponent<ASL.ASLObject>().SendFloatArray(flt);
                        });
                    }
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

    private bool isJoinableTeams(int teamId)
    {
        if (m_teams[teamId].GetNumMember() < numMemberPerTeam)
            return true;
        return false;
    }

    private bool isAllTeamValid()
    {
        bool result = true;
        foreach (Team t in m_teams)
        {
            if ((t.GetNumMember() != 0 && t.GetNumMember() < 2))
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
            readyText.color = (isAllTeamValid()) ? Color.green : Color.red;
            ResetAction();
        }
    }
}
