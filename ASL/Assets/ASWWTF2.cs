using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class ASWWTF2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject i in IterateOverChildObjects(gameObject))
        {
            Debug.Log(i.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            GameObject obj = Resources.Load<GameObject>(@"MyPrefabs\WTFCUBE");
            Debug.Log("Child Count: " + obj.transform.childCount);
        }
    }   

    IEnumerable<GameObject> IterateOverChildObjects(GameObject obj)
    {
        for (int i = 0; i < obj.transform.childCount; ++i) {
            GameObject child = obj.transform.GetChild(i).gameObject;
            yield return child;
            if (child.transform.childCount > 0)
            {
                foreach (GameObject childchild in IterateOverChildObjects(child))
                {
                    yield return childchild;
                }
            }
        }
        yield break;
    }

}
