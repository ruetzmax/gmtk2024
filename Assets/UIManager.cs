using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject buildUI;
    public GameObject playUI;
    public GameObject partButtonPrefab;
    public GameObject scrollViewContent;

    public GameObject infoMessage;
    public GameObject targetArrow;
    public GameObject healthIndicator;
    public float infoMessageDuration = 2.0f;
    private float infoMessageTimer = 0.0f;


    void Start()
    {
        instance = this;
    }

    public void Update()
    {
        updateInfoMessage();
        if(GameManager.instance.gameState == GameState.PLAY){
            updateTargetArrow();
            updateHealthIndicator();
        }
    }

    public void showBuildUI()
    {
        playUI.SetActive(false);
        buildUI.SetActive(true);
        Canvas.ForceUpdateCanvases();

    }
    public void hideBuildUI()
    {
        buildUI.SetActive(false);
    }
    public void showPlayUI()
    {
        buildUI.SetActive(false);
        playUI.SetActive(true);
        Canvas.ForceUpdateCanvases();
    }
    public void hidePlayUI()
    {
        playUI.SetActive(false);
    }

    public void showInfoMessage(string message)
    {
        infoMessage.GetComponent<TMP_Text>().text = message;
        infoMessageTimer = infoMessageDuration;
    }

    private void updateInfoMessage(){
        if(infoMessageTimer > 0){
            infoMessageTimer -= Time.deltaTime;
        }
        else{
            infoMessageTimer = 0;
        }
        float alpha = infoMessageTimer / infoMessageDuration;
        Color color = infoMessage.GetComponent<TMP_Text>().color;
        color.a = alpha;
        infoMessage.GetComponent<TMP_Text>().color = color;
    }

    private void updateTargetArrow(){
        Vector3 portalPosition = GameManager.instance.portalObject.transform.position;
        Vector3 shipPosition = GameManager.instance.shipObject.transform.position;
        Vector3 direction = portalPosition - shipPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        targetArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void updateHealthIndicator(){
        float health = Mathf.RoundToInt(GameManager.instance.shipObject.GetComponent<HealthManager>().getHealth());
        healthIndicator.GetComponent<TMP_Text>().text = "Health: " + health.ToString();
    }

    public void fillPartsView(List<Part> parts)
    {
        // remove previous part buttons
        foreach (Transform button in scrollViewContent.transform)
        {
            Destroy(button.gameObject);
        }

        // add new part buttons
        foreach (Part part in parts)
        {
            GameObject partButton = Instantiate(partButtonPrefab);
            partButton.transform.SetParent(scrollViewContent.transform, false);
            partButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.buildManager.setActivePart(part));
            
            if (part.prefab.tag == "Ship")
            {
                partButton.GetComponentInChildren<TMP_Text>().text = "Ship (Level " + (GameManager.instance.level - 1).ToString() + ")";
            }
            else
            {
                partButton.GetComponentInChildren<TMP_Text>().text = part.prefab.tag;
            }

            part.uiElement = partButton;
        }
        Canvas.ForceUpdateCanvases();


    }
}
