using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    Camera m_camera;
    public float speed = 5;
    public float despawnDist = 200;
    Rigidbody2D m_Rigidbody;
    float lifeTime = 0;
    public float despawnTime = 5;
    // float lowVelocityTime = 0;
    // public float kTime = 5;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.velocity = transform.up * speed;
        m_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > despawnTime)
        {
            Destroy(gameObject);
        }
        /* if (m_Rigidbody.velocity.magnitude <= 1)
        {
            lowVelocityTime += Time.deltaTime;
        } 
        if (lowVelocityTime >)*/
        if (Vector3.Distance(transform.position, m_camera.transform.position) > despawnDist)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collider2D other)
    {
        // Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
