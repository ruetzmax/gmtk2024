using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject shipObject;
    private BuildManager buildManager;
    private Ship ship;
    public GameState gameState = GameState.BUILD;
    public static GameManager instance;
    void Start()
    {
        buildManager = GameObject.FindGameObjectWithTag("BuildManager").GetComponent<BuildManager>();
        ship = GameObject.FindGameObjectWithTag("Ship").GetComponent<Ship>();
        instance = this;

        setGameState(GameState.BUILD);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            toggleGameState();
        }
        
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
            //ship.deactivated();
        }
        else if (state == GameState.PLAY)
        {
            buildManager.deactivated();
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
