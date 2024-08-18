using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public float Health = 10;
    public float speed = 10;
    Rigidbody2D m_Rigidbody;
    void Start()
    {
        m_Rigidbody = transform.GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState != GameState.PLAY)
        {
            return;
        }
        Vector3 shipPosition = GameManager.instance.shipObject.transform.position;
        Vector3 toSpaceShipDir = (shipPosition - transform.position).normalized;
        float angle = Vector3.SignedAngle(transform.up, toSpaceShipDir, Vector3.forward);
        transform.Rotate(Vector3.forward, angle);
        m_Rigidbody.velocity = transform.up * speed;
    }
}
