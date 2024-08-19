using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    // Start is called before the first frame update
    Transform spriteChild;
    public GameObject canonBall;
    public float cooldown = 0.3f;
    float timer = 0;
    BuildManager buildManager;

    void Start()
    {
        spriteChild = transform.Find("canon");
        buildManager = GameObject.FindGameObjectWithTag("BuildManager").GetComponent<BuildManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState != GameState.PLAY)
        {
            return;
        }
        bool connectedToBlock = buildManager.isPartPlacedAt(transform.position - transform.up);
        if (!connectedToBlock)
        {
            Destroy(gameObject);
        }
    }

    public void Rotate(Vector3 mouseDir)
    {
        mouseDir.z = 0;
        // Debug.Log(transform);
        // spriteChild = GetComponent<Sprite>();
        spriteChild = transform.Find("canon");
        // Debug.Log(spriteChild);
        float angle = Vector3.SignedAngle(transform.up, mouseDir, Vector3.forward);
        float spriteTuUp = Vector3.SignedAngle(spriteChild.up, transform.up, Vector3.forward);
        angle = Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), 70);
        // spriteChild.Rotate(0, 0, spriteTuUp + angle);
        spriteChild.RotateAround(transform.position- transform.up * transform.localScale.y / 2, Vector3.forward, spriteTuUp + angle);
    }

    public void Shoot()
    {
        timer += Time.time;
        if (timer < cooldown)
        {
            return;
        }
        timer = 0;

        Vector3 offset = spriteChild.up * 0.7f;
        Debug.Log("shoot");
        GameObject newCanonBall = Instantiate(canonBall, spriteChild.position + offset, spriteChild.rotation);

        Collider2D cannonCollider = GetComponent<Collider2D>();
        Collider2D canonBallCollider = newCanonBall.GetComponent<Collider2D>();
        Collider2D shipCollider = GameManager.instance.shipObject.GetComponent<Collider2D>();
        if (cannonCollider != null && canonBallCollider != null)
        {
            Physics2D.IgnoreCollision(cannonCollider, canonBallCollider);
            Physics2D.IgnoreCollision(shipCollider, canonBallCollider);
        }

        Rigidbody2D spaceshipRigidbody = GetComponentInParent<Rigidbody2D>();
        Rigidbody2D cannonballRigidbody = newCanonBall.GetComponent<Rigidbody2D>();
        if (spaceshipRigidbody != null && cannonballRigidbody != null)
        {
            cannonballRigidbody.velocity += spaceshipRigidbody.velocity;
        }
    }
}
