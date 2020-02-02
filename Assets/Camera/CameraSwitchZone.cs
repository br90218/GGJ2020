using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchZone : MonoBehaviour
{
    public int camNumber;
    public CameraController cameraController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(camNumber);
        cameraController.SetActiveCam(camNumber);
    }
}
