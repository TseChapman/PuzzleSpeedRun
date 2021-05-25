using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ASL;

public class Team : MonoBehaviour
{
    public int id;
    public TMP_Text teamMemberText;
    private List<int> m_memberId = new List<int>();
    private bool actionSignal = false;

    public void AddMember(int id)
    {
        if (m_memberId.Contains(id)) return;
        m_memberId.Add(id);
        actionSignal = true;
    }

    public int GetTeamId() { return id; }

    public int GetNumMember() { return m_memberId.Count; }
    public int GetMemberId(int index) { return m_memberId[index]; }

    public void RemoveMember(int id)
    {
        m_memberId.Remove(id);
        actionSignal = true;
    }

    private void UpdateText()
    {
        if (actionSignal == true)
        {
            Debug.Log("Team" + id + "Update Text");
            string text = "";
            for (int i = 0; i < m_memberId.Count; i++)
            {
                string username = GameLiftManager.GetInstance().m_Players[m_memberId[i]];
                text += username + "\n";
            }
            teamMemberText.text = text;
        }
        actionSignal = false;
    }

    private void Update()
    {
        // Update the text of the team member
        UpdateText();
    }
}
