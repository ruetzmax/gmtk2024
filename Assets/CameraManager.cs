using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.shipObject == null)
        {
            return;
        }
        
        Vector3 shipPosition = GameManager.instance.shipObject.transform.position;
        transform.position = new Vector3(shipPosition.x, shipPosition.y, -10);
    }
}
