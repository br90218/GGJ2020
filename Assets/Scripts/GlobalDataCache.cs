using PubSubCommunication;
using PubSubMessages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will be around always in every scene. It's job is to hold persistant data
/// between scenes.
/// </summary>
public class GlobalDataCache : GenericPubSubComponent
{
    public static GlobalDataCache instance;

    #region Public Fields

    public string PlayerName = "";
    public int Points = 0;
    public int Health = 0;
    public int Lives = 0;

    #endregion

#region Private Fields

    private Dictionary<int, object> cache;

#endregion

    //This code makes sure only one GlobalDataCache exists at a time.
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

    private void OnDestroy()
    {
    }

#region Pub Sub Functions

#endregion

}
