using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    public void DefaultButton()
    {
        AudioManager.instance.Play("ButtonClick");
    }

    public void Mute(string mixerGroup)
    {
        AudioManager.instance.Mute(mixerGroup);
    }

    public void Unmute(string mixerGroup)
    {
        AudioManager.instance.Unmute(mixerGroup);
    }
}
