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
        GameManager.instance.shipObject.transform.position = new Vector3(0, 0, 0);

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
    public void generateAvailableParts()
    {
        availableParts = new List<Part>();

        //only temporary, generate depending randomly and with previous ship versions
        addAvailablePart(tilePrefab);
        addAvailablePart(canonPrefab);
        addAvailablePart(thrusterPrefab);
    }
    public void setActivePart(Part part)
    {
        if (activePartObject != null)
        {
            Destroy(activePartObject);
        }
        activePart = part;
        activePartObject = Instantiate(activePart.prefab);
        Color spriteColor = activePartObject.GetComponent<SpriteRenderer>().color;
        spriteColor.a = 0.5f;
        activePartObject.GetComponent<SpriteRenderer>().color = spriteColor;
        activePartObject.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    public void setActivePart(GameObject prefab, GameObject uiElement = null)
    {
        setActivePart(new Part { prefab = prefab, uiElement = uiElement });
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
        Color spriteColor = activePartObject.GetComponent<SpriteRenderer>().color;
        spriteColor.a = 1.0f;
        activePartObject.GetComponent<SpriteRenderer>().color = spriteColor;

        FixedJoint2D joint = activePartObject.AddComponent<FixedJoint2D>();
        joint.connectedBody = GameManager.instance.shipObject.GetComponent<Rigidbody2D>();
        joint.enableCollision = false;
        activePartObject.transform.SetParent(GameManager.instance.shipObject.transform.Find("parts"));
        activePartObject.GetComponent<Rigidbody2D>().isKinematic = true;

        Destroy(activePart.uiElement);
        availableParts.Remove(activePart);

        activePart = null;
        activePartObject = null;


        //DEBUG
        // setActivePart(tilePrefab);
    }

}

public class Part{
    public GameObject prefab;
    public GameObject uiElement;
}
