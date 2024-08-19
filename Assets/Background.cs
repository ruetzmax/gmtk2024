using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject backgroundPreFab;
    public GameObject shipObject;
    void Start()
    {
        shipObject = GameManager.instance.shipObject;
    }

    // Update is called once per frame
    /*void Update()
    {
        if (shipObject.transform.position)
    }*/
}
