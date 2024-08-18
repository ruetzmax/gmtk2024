using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    public float speedThreshold = 5.0f;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
        if (rb != null && rb.velocity.magnitude > speedThreshold)
        {
            Destroy(gameObject);
        }
    }
}
