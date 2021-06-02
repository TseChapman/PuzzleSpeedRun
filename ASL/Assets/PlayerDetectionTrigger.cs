using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetectionTrigger : MonoBehaviour
{

    public UnityEvent OnPlayerEntered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        OnPlayerEntered.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
