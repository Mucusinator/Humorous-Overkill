using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {


    public int maxHealth;
    private int currentHealth;

    // Use this for initialization
	void Start () {

        currentHealth = maxHealth;
	}

    // Update is called once per frame
    //void Update()
    //{


    //}

   public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

   public void HealDamage(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
