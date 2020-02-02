using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSubCommunication;
using PubSubMessages;

public class LevelManager : GenericPubSubComponent
{
    protected static LevelManager instance;
    public static LevelManager Instance {
        get {
            if (instance == null) {
                var singleton = new GameObject("Level Manager", typeof(LevelManager));
                instance = singleton.GetComponent<LevelManager>();
            }
            return instance;
        }
    }

    public int score = 0;
    public int winScore = 0;
    public string nextLevel = null;
    public int nextLevelWinScore = 0;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(score >= winScore && score != 0)
        {
            SuccessMessage successMessage = new SuccessMessage();

            successMessage.nextLevel = nextLevel;
            successMessage.nextLevelWinScore = nextLevelWinScore;

            PubSubServerInstance.Publish(successMessage);

            score = 0;
            winScore = nextLevelWinScore;
        }
    }

    public void AddPoint()
    {
        score += 1;
    }
}
