using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubSubCommunication;
using PubSubMessages;

public class PlayButton : GenericPubSubComponent
{
    public void StartGame()
    {
        Debug.Log("PlayButton pressed, requesting game start");

        StartGameMessage startGameMessage = new StartGameMessage();

        PubSubServerInstance.Publish(startGameMessage);
    }
}
