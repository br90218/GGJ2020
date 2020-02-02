﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookController : MonoBehaviour
{
    public Transform hand;
    public LayerMask backGroundLayer;
    public LayerMask hingeLayer;
    public LayerMask obstacleLayer;
    public AnimationCurve grappleLaunchCurve;
    [Header("OtherController")] 
    public SwingStrafeController swingStrafeController;
    public PlayerController playerController;
    public RopeRenderer ropeRenderer;
    private LayerMask floorLayer;
    private bool onGrapple;
    private Vector3 grapplePoint;
    private float grappleLaunchTime;
    private float grappleLaunchTotalTime;


    private void Update()
    {
        if (onGrapple) {
            CheckGrappleHit();
        }
        else {
            CheckHingeHitUpdate();
        }
    }

    private void CheckGrappleHit()
    {
        Vector2 handPos = hand.position;
        Vector2 dir = (Vector2)grapplePoint - handPos;
        dir = dir.normalized;
        grappleLaunchTime += Time.deltaTime;
        if (grappleLaunchTotalTime < grappleLaunchTime)
        {
            //Debug.Log("Rope doesn't reach");
            ropeRenderer.ClearRope();
            onGrapple = false;
            return;
        }
        var distance = grappleLaunchCurve.Evaluate(grappleLaunchTime);
        
        RaycastHit hit;
        if (Physics.Raycast(handPos,dir,out hit,distance,hingeLayer))
        {
            ropeRenderer.MovePoint(0,handPos);
            ropeRenderer.MovePoint(1,hit.collider.transform.parent.position);
            ropeRenderer.RepaintRope();
            enabled = false;
            onGrapple = false;
            swingStrafeController.StartSwing(hit.collider.transform.parent);
            playerController.enabled = false;
//            ropeRenderer.ClearRope();
//            onGrapple = false;
        }
        else if (Physics.Raycast(handPos,dir,out hit,distance,obstacleLayer))
        {
            //Hit a obstacle
            ropeRenderer.ClearRope();
        } 
        //Nothing is hit yet
        else
        {
            ropeRenderer.MovePoint(0,handPos);
            ropeRenderer.MovePoint(1,handPos + dir*distance);
            ropeRenderer.RepaintRope();
        }

    }
    
    private void CheckHingeHitUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit,2000,backGroundLayer))
            {
                onGrapple = true;
                grapplePoint = hit.point;
                grappleLaunchTotalTime = grappleLaunchCurve[grappleLaunchCurve.length - 1].time;
                grappleLaunchTime = 0;
                ropeRenderer.ClearRope();
                ropeRenderer.AddPoint(Vector3.zero);
                ropeRenderer.AddPoint(Vector3.zero);
            }
        }
        
    }
}
