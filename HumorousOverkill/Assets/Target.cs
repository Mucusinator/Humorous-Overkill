using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    public float health = 50f;
    

    // Replace this with the game manager handler event "DealDamage".
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            // Replace this with the game manager handler event "KillEnemy".
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
