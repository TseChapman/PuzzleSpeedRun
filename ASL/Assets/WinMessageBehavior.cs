using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMessageBehavior : MonoBehaviour {
    public float winMessagetime = 7f;
    // Start is called before the first frame update
    private void Awake()
    {
        StartCoroutine(closeWinMessage());
    }
    IEnumerator closeWinMessage()
    {
        yield return new WaitForSeconds(winMessagetime);
        gameObject.SetActive(false);
    }

}
