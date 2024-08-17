using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject buildUI;
    public GameObject partButtonPrefab;
    public GameObject scrollViewContent;

    void Start()
    {
        instance = this;
    }

    public void showBuildUI()
    {
        buildUI.SetActive(true);
    }
    public void hideBuildUI()
    {
        buildUI.SetActive(false);
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
            partButton.transform.SetParent(scrollViewContent.transform);
            partButton.GetComponentInChildren<TMP_Text>().text = part.prefab.tag;
            partButton.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.buildManager.setActivePart(part));
            part.uiElement = partButton;
            Canvas.ForceUpdateCanvases();
        }

    }
}
