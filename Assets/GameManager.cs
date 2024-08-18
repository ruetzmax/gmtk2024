using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int portalDistance = 15;
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

        if (gameState == GameState.END && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("main");
        }
        
    }

    public void nextLevel()
    {        
        //update values
        level++;
        portalDistance = (int)(portalDistance * maxPortalDistanceMultiplier);
        //reset ship
        shipObject.transform.position = new Vector3(0, 0, 0);
        shipObject.transform.rotation = Quaternion.identity;
        shipObject.GetComponent<HealthManager>().resetHealth();
    
        //new portal
        if (portalObject != null)
        {
            Destroy(portalObject);
        }
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector2 portalPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * portalDistance;;
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

        ship.setPositionToTileAvg();
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

    public void objectKilled(GameObject obj)
    {
        if (obj.tag == "Ship")
        {
            gameState = GameState.END;
            UIManager.instance.hidePlayUI();
            Destroy(obj);
            UIManager.instance.showInfoMessage("You died! Press SPACE to restart.");

        }
    }
}


public enum GameState
{
    BUILD,
    PLAY,
    END
}
