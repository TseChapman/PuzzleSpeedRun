using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    public Transform[] spawnPositions;
    private int emptyPositionIndex = 0;

    public void ResetEmptyPosIndex() { emptyPositionIndex = 0; }

    public Vector3 GetEmptySpawnPosition()
    {
        Vector3 pos = new Vector3(1000f, 1000f, 1000f);
        if (emptyPositionIndex < spawnPositions.Length)
        {
            pos = spawnPositions[emptyPositionIndex].position;
            pos.y += 1f;
            emptyPositionIndex++;
        }
        return pos;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
