using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingStrafeController : MonoBehaviour
{

    public RopeRenderer ropeRenderer;
    public Transform hand;

    private Vector3 hingePoint;
    public void StartSwing(Vector3 hingePoint)
    {
        enabled = true;
        this.hingePoint = hingePoint;
    }

    private void Update()
    {
        throw new NotImplementedException();
    }
}
