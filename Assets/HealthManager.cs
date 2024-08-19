using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    
    private float health;
    public float maximumHealth = 100;
    public float collisionDamageMultiplier = 0.5f;
    void Start()
    {
        resetHealth();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float speed = collision.relativeVelocity.magnitude;
        health -= speed * collisionDamageMultiplier;

        if(health <= 0)
        {
            GameManager.instance.objectKilled(gameObject);
        } 
    }

    public float getHealth()
    {
        return health;
    }

    public void resetHealth()
    {
        health = maximumHealth;
    }
}
