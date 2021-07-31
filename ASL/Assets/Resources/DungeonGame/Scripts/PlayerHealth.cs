using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int power = 20;
    public int currentHealth;

    public HealthBarCalc healthBar;
    public MobHealth MOBhealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100.0f, Color.yellow);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            takeDamage(20);
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50f, 1 << LayerMask.NameToLayer("MOB")))
            {
                hit.transform.GetComponent<MobHealth>().takeDamage(power);
            }
        }
    }

    void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }
}
