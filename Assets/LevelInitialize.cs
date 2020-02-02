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
        LevelManager.Instance.nextLevel = nextLevel;
        LevelManager.Instance.nextLevelWinScore = nextLevelWinScore;

        PopupManager.instance.SuccessText.SetActive(false);
        PopupManager.instance.FailureText.SetActive(false);
    }
}
