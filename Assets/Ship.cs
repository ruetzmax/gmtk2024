using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    // Start is called before the first frame update
    public float thrusterStrength = 5;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState != GameState.PLAY)
        {
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            activateThrusters(Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            activateThrusters(Vector3.left);
        }
        if (Input.GetKey(KeyCode.S))
        {
            activateThrusters(Vector3.down);
        }
        if (Input.GetKey(KeyCode.D))
        {
            activateThrusters(Vector3.right);
        }
        // Canon
        // get mouse position
        // loop over all canons
        // check that Angle between canon.up and mouse_position - canon_position
        // Rotate towards mouse / around that angle but with a maximum allowed rotation
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rotateCanons(mousePos);
        if (Input.GetMouseButtonDown(0))
        {
            shootCanons();
        }
        setPositionToTileAvg();

    }

    void setPositionToTileAvg()
    {
        Vector3 avgPosition = Vector3.zero;
        int numParts = transform.Find("parts").childCount;
        for (int i = 0; i < numParts; i++)
        {
            Transform shipPart = transform.Find("parts").GetChild(i);
            avgPosition += shipPart.position;
        }
        avgPosition /= numParts;
        Vector3 parentOffset = avgPosition - transform.position;

        transform.position = avgPosition;

        // Adjust the children's positions to keep them in the same world position
        for (int i = 0; i < numParts; i++)
        {
            Transform shipPart = transform.Find("parts").GetChild(i);
            shipPart.position -= parentOffset;
        }
    }

    void activateThrusters(Vector3 direction)
    {
        for (int i = 0; i < transform.Find("parts").childCount; i++)
        {
            Transform shipPart = transform.Find("parts").GetChild(i);
            if (shipPart.tag != "Thruster")
            {
                continue;
            }
            Thruster thruster = shipPart.GetComponent<Thruster>();
            float angle = Vector3.Angle(shipPart.up, direction);
            Debug.Log(angle);
            if (angle > 50)
            {
                continue;
            }
            Debug.Log("Firing");
            thruster.Fire(thrusterStrength);
        }
    }

    void rotateCanons(Vector3 mousePos)
    {
        for (int i = 0; i < transform.Find("parts").childCount; i++)
        {
            Transform shipPart = transform.Find("parts").GetChild(i);
            if (shipPart.tag != "Canon")
            {
                continue;
            }
            // Debug.Log(shipPart);
            Canon canon = shipPart.GetComponent<Canon>();
            // Debug.Log(canon);
            Vector3 mouseDir = mousePos - shipPart.position;
            // Debug.Log(mouseDir);
            canon.Rotate(mouseDir);
        }
    }

    void shootCanons()
    {
        for (int i = 0; i < transform.Find("parts").childCount; i++)
        {
            Transform shipPart = transform.Find("parts").GetChild(i);
            if (shipPart.tag != "Canon")
            {
                continue;
            }
            Canon canon = shipPart.GetComponent<Canon>();
            canon.Shoot();
        }
    }
}
