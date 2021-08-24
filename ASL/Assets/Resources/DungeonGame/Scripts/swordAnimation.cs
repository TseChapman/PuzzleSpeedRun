using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordAnimation : MonoBehaviour
{
    Animator animator;
    public int power = 20;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("swingSword");
        }
        else
        {
            animator.ResetTrigger("swingSword");
        }
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    //col.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //    //col.transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    //    //layer 25 is MOB
    //    if (col.gameObject.layer == 25  && this.animator.GetCurrentAnimatorStateInfo(0).IsName("swing"))
    //    {
    //        col.transform.GetComponent<MobHealth>().takeDamage(power);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER");
        if (other.gameObject.layer == 25 && this.animator.GetCurrentAnimatorStateInfo(0).IsName("swing"))
        {
            other.transform.GetComponent<MobHealth>().takeDamage(power);
        }
    }
}
