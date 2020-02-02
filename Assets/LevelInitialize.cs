using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitialize : MonoBehaviour
{
    public string nextLevel;
    public int nextLevelWinScore;

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.nextLevel = nextLevel;
        LevelManager.instance.nextLevelWinScore = nextLevelWinScore;
    }
}
