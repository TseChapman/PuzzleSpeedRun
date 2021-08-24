using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public GameObject myPrefab;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(myPrefab, new Vector3(2, (float) 1.5, 0), Quaternion.identity);
        }
    }
}
