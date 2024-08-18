using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject activePartPrefab;


    public GameObject tilePrefab;
    public GameObject canonPrefab;
    public GameObject thrusterPrefab;

    private GameObject activePartObject;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.gameState != GameState.BUILD)
        {
            return;
        }  

        updateActivePartPosition();

        if (Input.GetMouseButtonDown(0))
        {
            placeActivePart();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            setActivePart(canonPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            setActivePart(thrusterPrefab);
        }

    }

    public void deactivated(){
        activePartPrefab = null;
        Destroy(activePartObject);
    }

    public void activated() {
        setActivePart(tilePrefab);
    }

    public void setActivePart(GameObject part)
    {
        if (activePartObject != null)
        {
            Destroy(activePartObject);
        }
        activePartPrefab = part;
        activePartObject = Instantiate(activePartPrefab);
        Color spriteColor = activePartObject.GetComponent<SpriteRenderer>().color;
        spriteColor.a = 0.5f;
        activePartObject.GetComponent<SpriteRenderer>().color = spriteColor;
        activePartObject.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    private void updateActivePartPosition()
    {
       if (activePartPrefab == null || activePartObject == null)
       {
           return;
       }
    //aaaaa
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        activePartObject.transform.position = new Vector2(Mathf.Round(objectPosition.x), Mathf.Round(objectPosition.y));
    }

    private bool isPartPlacedAt(Vector3 position){
        for (int i = 0; i < GameManager.instance.shipObject.transform.Find("parts").childCount; i++)
        {
            Transform shipPart = GameManager.instance.shipObject.transform.Find("parts").GetChild(i);
            if(shipPart.transform.position == position)
            {
                return true;
            }

        }
        return false;
    }

    private bool isValidPlace()
    {
        // neighbours
        if( !isPartPlacedAt(activePartObject.transform.position + new Vector3(1, 0, 0)) &&
            !isPartPlacedAt(activePartObject.transform.position + new Vector3(-1, 0, 0)) &&
            !isPartPlacedAt(activePartObject.transform.position + new Vector3(0, 1, 0)) &&
            !isPartPlacedAt(activePartObject.transform.position + new Vector3(0, -1, 0)))
        {
            return false;
        }

        //on top
        if (isPartPlacedAt(activePartObject.transform.position))
        {
            return false;
        }

        return true;
    }

    private void placeActivePart()
    {
        if (activePartPrefab == null || activePartObject == null)
        {
            return;
        }

        if (!isValidPlace() && GameManager.instance.shipObject.transform.Find("parts").childCount > 0)
        {
            return;
        }

        //place part
        Color spriteColor = activePartObject.GetComponent<SpriteRenderer>().color;
        spriteColor.a = 1.0f;
        activePartObject.GetComponent<SpriteRenderer>().color = spriteColor;

        FixedJoint2D joint = activePartObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = GameManager.instance.shipObject.GetComponent<Rigidbody2D>();
        joint.enableCollision = false;
        activePartObject.transform.SetParent(GameManager.instance.shipObject.transform.Find("parts"));
        activePartObject.GetComponent<Rigidbody2D>().isKinematic = true;

        activePartPrefab = null;
        activePartObject = null;


        //DEBUG
        setActivePart(tilePrefab);
    }

}
