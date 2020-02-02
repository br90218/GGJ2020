using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSubMessages;
using PubSubCommunication;

public class PopupManager : GenericPubSubComponent
{
    public static PopupManager instance;

    public GameObject SuccessText;
    public GameObject FailureText;

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

        PubSubServerInstance.Subscribe(typeof(SuccessMessage), OnSuccess);
        PubSubServerInstance.Subscribe(typeof(FailureMessage), OnFailure);
    }

    private void OnDestroy()
    {
        PubSubServerInstance.Unsubscribe(typeof(SuccessMessage), OnSuccess);
        PubSubServerInstance.Unsubscribe(typeof(FailureMessage), OnFailure);
    }

    void OnSuccess(BaseMessage m)
    {
        SuccessText.SetActive(true);
    }

    void OnFailure(BaseMessage m)
    {
        FailureText.SetActive(true);
    }
}
