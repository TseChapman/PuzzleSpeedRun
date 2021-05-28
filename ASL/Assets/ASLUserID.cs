using ASL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASLUserID
{

    private static ASLUserID instance = null;

    private int id;

    private ASLUserID()
    {
        UpdateID();
    }

    public static int ID()
    {
        if (instance == null)
        {
            instance = new ASLUserID();
        }
        return instance.GetID();
    }

    int GetID()
    {
        UpdateID();
        return id;
    }

    void UpdateID()
    {
        GameLiftManager glm = GameLiftManager.GetInstance();
        List<string> usernames = new List<string>();
        foreach (string username in glm.m_Players.Values)
        {
            usernames.Add(username);
            //Debug.Log("Username: " + username);
        }
        usernames.Sort();
        id = usernames.IndexOf(glm.m_Username);
    }
}
