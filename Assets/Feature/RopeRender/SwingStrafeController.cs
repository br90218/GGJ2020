using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingStrafeController : MonoBehaviour
{
    public float speedToAngular = 1;
    public float angularToSpeed = 0.8f;
    public float swingAcceleration = 1f;
    [Header("Controller and stuff")]
    public RopeRenderer ropeRenderer;
    public Transform hand;
    public PlayerController playerController;
    
    
    private float angularSpeed;
    private float angle;
    private Vector3 hingePoint;
    public void StartSwing(Vector3 hingePoint)
    {
        enabled = true;
        this.hingePoint = hingePoint;
        angularSpeed = playerController.Velocity.magnitude * speedToAngular;
        //Temp
        angularSpeed = 1;
        var dir = hingePoint - hand.position;
        //dir.y *= -1;
        angle = Mathf.Asin(dir.y/ dir.x);

    }
    private void Update()
    {
        float rightAxis = Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
        rightAxis = Input.GetKey(KeyCode.LeftArrow) ? -1f : rightAxis;
        angularSpeed += swingAcceleration * Time.deltaTime;
        

    }
}
