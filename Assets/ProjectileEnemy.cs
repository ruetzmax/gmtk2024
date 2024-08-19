using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy
{
    // Start is called before the first frame update
    public float shootSpeed = 2;
    public GameObject projectilePrefab;
    // public GameObject shipObject;
    public float timer = 0.0f;
    public float wantedDistance = 5;

    void Start()
    {
        m_Rigidbody = transform.GetComponent<Rigidbody2D>();
        m_Rigidbody.drag = drag;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState != GameState.PLAY)
        {
            return;
        }
        // move towards a bit away from neirest point
        GameObject shipObject = GameManager.instance.shipObject;
        Collider2D shipCollider = shipObject.GetComponent<Collider2D>();
        Debug.Log(shipCollider);
        if (shipCollider == null)
        {
            return;
        }
        Vector3 closestPoint = shipCollider.ClosestPoint(transform.position);
        Rigidbody2D m_Rigidbody = GetComponent<Rigidbody2D>();
        Vector3 wantedPoint = closestPoint + (transform.position - closestPoint).normalized * wantedDistance;
        Vector3 forceDir = (wantedPoint - transform.position).normalized;
        m_Rigidbody.AddForce(forceDir * speed);

        Vector3 toSpaceShipDir = closestPoint - transform.position;
        float angle = Vector3.SignedAngle(transform.up, toSpaceShipDir, Vector3.forward);
        transform.Rotate(Vector3.forward, angle);

        if (timer < shootSpeed)
        {
            timer += Time.deltaTime;
            return;
        }
        timer = 0.0f;

        // Shoot
        Shoot();
    }

    public void Shoot()
    {
        Debug.Log("Shoot!");
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Collider2D shooterCollider = GetComponent<Collider2D>();
        Collider2D bulletCollider = projectile.GetComponent<Collider2D>();
        if (shooterCollider != null && bulletCollider != null)
        {
            Physics2D.IgnoreCollision(shooterCollider, bulletCollider);
        }
        Rigidbody2D m_Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody2D projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
        if (m_Rigidbody != null && projectileRigidbody != null)
        {
            projectileRigidbody.velocity += m_Rigidbody.velocity;
        }
    }
}
