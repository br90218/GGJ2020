using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSubCommunication;
using PubSubMessages;

public class DeathZone : MonoBehaviour
{
    void Death()
    {
        Debug.Log("Level Failed");

        FailureMessage failureMessage = new FailureMessage();

        Debug.Log("Publishing Failure Message");
        PubSubServer.Instance.Publish(failureMessage);

        AudioManager.instance.Play("Fail");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Death();
    }
}
