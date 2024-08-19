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

    private Level currLevel;
    [HideInInspector]
    public int enemyKillCount = 0;

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
        level++;
        startLevel(level);
    }

    void startLevel(int level)
    {
        if (currLevel != null)
        {
            currLevel.endLevel();
        }
        //update values
        portalDistance = (int)(portalDistance * maxPortalDistanceMultiplier);
        //reset ship
        shipObject.transform.position = new Vector3(0, 0, 0);
        shipObject.transform.rotation = Quaternion.identity;
        

        //new portal
        if (portalObject != null)
        {
            Destroy(portalObject);
        }

        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector2 portalPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * portalDistance;
        portalObject = Instantiate(portalPrefab, new Vector3(portalPosition.x, portalPosition.y, 0), Quaternion.identity);
        // instantiate Level and spawn Enemies
        currLevel = transform.Find("Level" + level.ToString()).GetComponent<Level>();
        if (currLevel != null)
        {
            currLevel.startLevel();

            //update game state
            buildManager.generateAvailableParts(level);
            setGameState(GameState.BUILD);
        }
        else
        {
            gameWon();
        }
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

        ship.setHealthByPartCount();
        shipObject.GetComponent<HealthManager>().resetHealth();

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
        } else
        {
            enemyKillCount++;
            Destroy(obj);
        }
    }
    public void gameWon()
    {
        Destroy(shipObject);    
        UIManager.instance.showEndScreen();
        gameState = GameState.END;
    }
}


public enum GameState
{
    BUILD,
    PLAY,
    END
}
