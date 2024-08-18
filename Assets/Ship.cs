using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    // Start is called before the first frame update
    public float thrusterMovementStrength = 2;
    public float thrusterRotationStrength = 0.5f;

    public float baseMovementForce = 3;
    public float baseRotationForce = 3;
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
        handleThrusters();
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

    public void setPositionToTileAvg()
    {
        Vector3 avgPosition = Vector3.zero;
        Transform closestPart = null;
        float closestDistance = float.MaxValue;

        int numParts = transform.Find("parts").childCount;
        for (int i = 0; i < numParts; i++)
        {
            Transform shipPart = transform.Find("parts").GetChild(i);
            avgPosition += shipPart.position;

            float distance = Vector3.Distance(avgPosition / (i + 1), shipPart.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPart = shipPart;
            }
        }
        avgPosition /= numParts;

        Vector3 parentOffset = closestPart.position - transform.position;
        transform.position = closestPart.position;

        // Adjust the children's positions to keep them in the same world position
        for (int i = 0; i < numParts; i++)
        {
            Transform shipPart = transform.Find("parts").GetChild(i);
            shipPart.position -= parentOffset;
        }
    }

    // void activateThrusters(Vector3 direction)
    // {
    //     for (int i = 0; i < transform.Find("parts").childCount; i++)
    //     {
    //         Transform shipPart = transform.Find("parts").GetChild(i);
    //         if (shipPart.tag != "Thruster")
    //         {
    //             continue;
    //         }
    //         Thruster thruster = shipPart.GetComponent<Thruster>();
    //         float angle = Vector3.Angle(shipPart.up, direction);
    //         Debug.Log(angle);
    //         if (angle > 50)
    //         {
    //             continue;
    //         }
    //         Debug.Log("Firing");
    //         thruster.Fire(thrusterStrength);
    //     }
    // }

     void handleThrusters()
    {
        int upCount = 0;
        int downCount = 0;
        int leftCount = 0;
        int rightCount = 0;
        for (int i = 0; i < transform.Find("parts").childCount; i++)
        {
            Transform shipPart = transform.Find("parts").GetChild(i);
            if (shipPart.tag != "Thruster")
            {
                continue;
            }
            float rotation = shipPart.transform.localEulerAngles.z;
            if (rotation % 360 == 0)
            {
                upCount++;
            }
            else if (rotation % 360 == 180)
            {
                downCount++;
            }
            else if (rotation % 360 == 90)
            {
                leftCount++;
            }
            else if (rotation % 360 == 270)
            {
                rightCount++;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * (baseMovementForce + upCount*thrusterMovementStrength));
        }
        if (Input.GetKey(KeyCode.S))
        {
            GetComponent<Rigidbody2D>().AddForce(-transform.up * (baseMovementForce + downCount*thrusterMovementStrength));
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().AddTorque(baseRotationForce + leftCount*thrusterRotationStrength);
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().AddTorque(-(baseRotationForce + rightCount*thrusterRotationStrength));
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
