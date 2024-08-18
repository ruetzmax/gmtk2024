using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    // Start is called before the first frame update
    Transform spriteChild;
    public GameObject canonBall;
    void Start()
    {
        spriteChild = transform.Find("canon");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotate(Vector3 mouseDir)
    {
        mouseDir.z = 0;
        // Debug.Log(transform);
        spriteChild = transform.Find("canon");
        // Debug.Log(spriteChild);
        float angle = Vector3.SignedAngle(transform.up, mouseDir, Vector3.forward);
        float spriteTuUp = Vector3.SignedAngle(spriteChild.up, transform.up, Vector3.forward);
        angle = Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), 70);
        spriteChild.Rotate(0, 0, spriteTuUp + angle);
    }

    public void Shoot()
    {
        Vector3 offset = spriteChild.up * 0.2f;
        GameObject newCanonBall = Instantiate(canonBall, spriteChild.position + offset, spriteChild.rotation);

        Collider2D cannonCollider = GetComponent<Collider2D>();
        Collider2D canonBallCollider = newCanonBall.GetComponent<Collider2D>();
        if (cannonCollider != null && canonBallCollider != null)
        {
            Physics2D.IgnoreCollision(cannonCollider, canonBallCollider);
        }

        Rigidbody2D spaceshipRigidbody = GetComponentInParent<Rigidbody2D>();
        Rigidbody2D cannonballRigidbody = newCanonBall.GetComponent<Rigidbody2D>();
        if (spaceshipRigidbody != null && cannonballRigidbody != null)
        {
            cannonballRigidbody.velocity += spaceshipRigidbody.velocity;
        }
    }
}
