using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera[] cams;
    public CameraSwitchZone[] zones;
    public int startCamNum = 0;

    public void SetActiveCam(int i)
    {
        foreach (var cam in cams)
        {
            cam.Priority = 8;
        }
        cams[i].Priority = 9;
    }
    private void Awake()
    {
        for (int i = 0; i < cams.Length; i++)
        {
            zones[i].camNumber = i;
            zones[i].cameraController = this;
        }
        SetActiveCam(startCamNum);
    }
    
    
}
