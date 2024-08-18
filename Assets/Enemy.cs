using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float Health = 10;
    public float speed = 10;
    public Rigidbody2D m_Rigidbody;
    public float drag = 1.0f;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
