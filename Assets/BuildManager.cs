using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public Part activePart;


    public GameObject tilePrefab;
    public GameObject canonPrefab;
    public GameObject thrusterPrefab;
    public List<Part> availableParts;

    private GameObject activePartObject;
    [HideInInspector]
    public GameObject previousShip;
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
        resetShip();

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            placeActivePart();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            rotateActivePart(true);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            rotateActivePart(false);
        }


    }

    public void deactivated(){
        UIManager.instance.hideBuildUI();
        activePart = null;
        Destroy(activePartObject);
    }

    public void activated() {
        UIManager.instance.fillPartsView(availableParts);
        UIManager.instance.showBuildUI();
    }

    private void addAvailablePart(GameObject prefab, GameObject uiElement = null){
        availableParts.Add(new Part { prefab = prefab, uiElement = uiElement });
    }
    public void generateAvailableParts(int level)
    {
        availableParts = new List<Part>();

        if (level == 1)
        {
            addAvailablePart(tilePrefab);
            addAvailablePart(canonPrefab);
            addAvailablePart(thrusterPrefab);
            return;
        }

        //always add a tile
        addAvailablePart(tilePrefab);

        //50% chance for each canon and thruster
        if (Random.value < 0.5f)
        {
            addAvailablePart(canonPrefab);
        }
        if (Random.value < 0.5f)
        {
            addAvailablePart(thrusterPrefab);
        }

        addAvailablePart(previousShip);
    }
    public void setActivePart(Part part)
    {
        if (activePartObject != null)
        {
            Destroy(activePartObject);
        }
        activePart = part;
        activePartObject = Instantiate(activePart.prefab);
        if (activePartObject.tag == "Ship")
        {
            activePartObject.SetActive(true);
            foreach (Transform shipPart in activePartObject.transform.Find("parts"))
            {
                Color spriteColor = shipPart.GetComponent<SpriteRenderer>().color;
                spriteColor.a = 0.5f;
                shipPart.GetComponent<SpriteRenderer>().color = spriteColor;
                shipPart.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else{
            Color spriteColor = activePartObject.GetComponent<SpriteRenderer>().color;
            spriteColor.a = 0.5f;
            activePartObject.GetComponent<SpriteRenderer>().color = spriteColor;
            activePartObject.GetComponent<Collider2D>().isTrigger = true;
        }
        

    }

    public void setActivePart(GameObject prefab, GameObject uiElement = null)
    {
        setActivePart(new Part { prefab = prefab, uiElement = uiElement });
    }

    private void resetShip(){
        GameManager.instance.shipObject.transform.position = new Vector3(0, 0, 0);
        GameManager.instance.shipObject.transform.rotation = Quaternion.identity;
        Rigidbody2D rb = GameManager.instance.shipObject.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        foreach (Transform part in GameManager.instance.shipObject.transform.Find("parts"))
        {
            part.transform.position = new Vector3(Mathf.Round(part.transform.position.x), Mathf.Round(part.transform.position.y), 0);
        }
    }

    private void updateActivePartPosition()
    {
       if (activePart == null || activePartObject == null)
       {
           return;
       }
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        activePartObject.transform.position = new Vector2(Mathf.Round(objectPosition.x), Mathf.Round(objectPosition.y));
    }

    private bool isPartPlacedAt(Vector3 position){
        for (int i = 0; i < GameManager.instance.shipObject.transform.Find("parts").childCount; i++)
        {
            Transform shipPart = GameManager.instance.shipObject.transform.Find("parts").GetChild(i);
            if (Vector3.Distance(shipPart.transform.position, position) < 0.05f)
            {
                return true;
            }

        }
        return false;
    }

    private bool isValidPlace()
    {
        if (activePartObject.tag == "Ship")
        {
            // on top
            foreach (Transform part in activePartObject.transform.Find("parts"))
            {
                if (isPartPlacedAt(part.transform.position))
                {
                    return false;
                }
            }
            // neighbours
            foreach (Transform part in activePartObject.transform.Find("parts"))
            {
                if (isPartPlacedAt(part.transform.position + new Vector3(1, 0, 0)) ||
                    isPartPlacedAt(part.transform.position + new Vector3(-1, 0, 0)) ||
                    isPartPlacedAt(part.transform.position + new Vector3(0, 1, 0)) ||
                    isPartPlacedAt(part.transform.position + new Vector3(0, -1, 0)))
                {
                    return true;
                }
            }
            return false;

        }
        else{
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
    }

    private void rotateActivePart(bool clockwise = true)
    {
        if (activePart == null || activePartObject == null)
        {
            return;
        }

        if (clockwise)
        {
            activePartObject.transform.Rotate(0, 0, -90);
        }
        else
        {
            activePartObject.transform.Rotate(0, 0, 90);
        }
    }

    private void placeActivePart()
    {
        if (activePart == null || activePartObject == null)
        {
            return;
        }

        if (!isValidPlace() && GameManager.instance.shipObject.transform.Find("parts").childCount > 0)
        {
            return;
        }

        //place part
        if (activePartObject.tag == "Ship")
        {
            List<Transform> shipParts = new List<Transform>();
            foreach (Transform child in activePartObject.transform.Find("parts"))
            {
                shipParts.Add(child);
            }
            foreach (Transform shipPart in shipParts)
            {
                Color spriteColor = shipPart.GetComponent<SpriteRenderer>().color;
                spriteColor.a = 1.0f;
                shipPart.GetComponent<SpriteRenderer>().color = spriteColor;

                // FixedJoint2D joint = shipPart.gameObject.AddComponent<FixedJoint2D>();
                // joint.connectedBody = GameManager.instance.shipObject.GetComponent<Rigidbody2D>();
                // joint.enableCollision = false;
                shipPart.transform.SetParent(GameManager.instance.shipObject.transform.Find("parts"));
                shipPart.GetComponent<Collider2D>().isTrigger = false;
            }
            Destroy(activePartObject);
        }
        else
        {
            Color spriteColor = activePartObject.GetComponent<SpriteRenderer>().color;
            spriteColor.a = 1.0f;
            activePartObject.GetComponent<SpriteRenderer>().color = spriteColor;

            // FixedJoint2D joint = activePartObject.AddComponent<FixedJoint2D>();
            // joint.connectedBody = GameManager.instance.shipObject.GetComponent<Rigidbody2D>();
            // joint.enableCollision = false;
            activePartObject.transform.SetParent(GameManager.instance.shipObject.transform.Find("parts"));
            activePartObject.GetComponent<Collider2D>().isTrigger = false;
        }
        

        Destroy(activePart.uiElement);
        availableParts.Remove(activePart);

        activePart = null;
        activePartObject = null;

    }

}

public class Part{
    public GameObject prefab;
    public GameObject uiElement;
}
