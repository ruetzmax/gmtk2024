using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D m_Rigidbody;
    public float drag = 1.0f;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.drag = drag;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Fire(float thrusterStrength)
    {
        Debug.Log("Adding force");
        m_Rigidbody.AddForce(transform.up * thrusterStrength);
    }
}
