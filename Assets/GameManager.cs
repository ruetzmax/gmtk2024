using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject shipObject;
    public GameObject portalPrefab;
    [HideInInspector]
    public GameObject portalObject;
    [HideInInspector]
    public BuildManager buildManager;
    private Ship ship;
    [HideInInspector]
    public GameState gameState = GameState.BUILD;
    public static GameManager instance;
    //level config
    [HideInInspector]
    public int level = 0;
    public int maxPortalDistance = 10;
    public float maxPortalDistanceMultiplier = 1.5f;

    void Start()
    {
        buildManager = GameObject.FindGameObjectWithTag("BuildManager").GetComponent<BuildManager>();
        ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<Ship>();
        instance = this;

        nextLevel();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            toggleGameState();
        }
        
    }

    public void nextLevel()
    {        
        //update values
        level++;
        maxPortalDistance = (int)(maxPortalDistance * maxPortalDistanceMultiplier);
        //reset ship
        shipObject.transform.position = new Vector3(0, 0, 0);
        shipObject.transform.rotation = Quaternion.identity;
    
        //new portal
        if (portalObject != null)
        {
            Destroy(portalObject);
        }
        Vector2 portalPosition = Random.insideUnitCircle * maxPortalDistance;
        portalObject = Instantiate(portalPrefab, new Vector3(portalPosition.x, portalPosition.y, 0), Quaternion.identity);

        //update game state
        buildManager.generateAvailableParts(level);
        setGameState(GameState.BUILD);
    }

    public void finishedBuilding()
    {
        if (buildManager.availableParts.Count > 0)
        {
            UIManager.instance.showInfoMessage("You have to use all parts!");
            return;
        }

        buildManager.previousShip = Instantiate(shipObject);
        buildManager.previousShip.SetActive(false);

        setGameState(GameState.PLAY);
        UIManager.instance.showInfoMessage("Level " + level);
    }

    void toggleGameState()
    {
        if (gameState == GameState.BUILD)
        {
            setGameState(GameState.PLAY);
        }
        else
        {
            setGameState(GameState.BUILD);
        }
    }

    void setGameState(GameState state)
    {
        if (state == GameState.BUILD)
        {
            buildManager.activated();
            UIManager.instance.showBuildUI();
            //ship.deactivated();
        }
        else if (state == GameState.PLAY)
        {
            buildManager.deactivated();
            UIManager.instance.showPlayUI();
            //ship.activated();
        }
        gameState = state;
    }
}

public enum GameState
{
    BUILD,
    PLAY
}
