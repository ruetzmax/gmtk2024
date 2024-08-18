using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tile" || other.gameObject.tag == "Canon" || other.gameObject.tag == "Thruster")
        {
            GameManager.instance.nextLevel();
        }
    }
    
}
