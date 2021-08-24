using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBarCalc healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //if (transform.GetComponent<Rigidbody>().isKinematic == true)
        //{
        //    transform.GetComponent<Rigidbody>().isKinematic = false;
        //}
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
