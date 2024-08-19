using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgoundImage : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject backgrondPrefab;
    SpriteRenderer spriteRenderer;
    float imgWidth;
    float imgHeight;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        imgWidth = spriteRenderer.size.x - 0.05f;
        imgHeight = spriteRenderer.size.y - 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Is the collision on the top / bottum / left or right?
        if (collision.tag != "Ship"){
            return;
        }
        // TODO: Add Tag
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3 newPos = transform.position + new Vector3(imgWidth * x, imgHeight * y, 0);
                if (!backgroundImgAtPosition(newPos))
                {
                    Instantiate(backgrondPrefab, newPos, transform.rotation);
                }
            }
        }
    }

    private bool backgroundImgAtPosition(Vector3 position)
    {
        GameObject[] backgroundImgs = GameObject.FindGameObjectsWithTag("Background");
        foreach (GameObject backgroundImg in backgroundImgs)
        {
            if (backgroundImg.transform.position == position)
            {
                return true;
            }
        }
        return false;
    }
}
