using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    Dictionary<int, Dictionary<string, int>> levelValues;
    void Start()
    {
        Dictionary<string, int> level0 = new Dictionary<string, int>();
        Dictionary<string, int> level1 = new Dictionary<string, int>();
        Dictionary<string, int> level2 = new Dictionary<string, int>();
        level0.Add("Flies", 1);
        level0.Add("Beetle", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
