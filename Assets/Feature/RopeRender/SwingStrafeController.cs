using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingStrafeController : MonoBehaviour
{
    public float speedToAngular = 1;
    public float angularToSpeed = 0.8f;
    public float swingAcceleration = 1f;
    public float maxAngularSpeed = 4.4f;
    public float angularDrag = 0.5f;
    [Header("Controller and stuff")]
    public RopeRenderer ropeRenderer;
    public Transform hand;
    public GrappleHookController grappleHookController;
    public PlayerController playerController;
    
    
    private float angularSpeed;
    private float angle;
    private Transform hingePoint;

    private float energyTotal;
    private float rotationEnergy;
    private float gravitationalPotentialEnergy;
    private float ropeLength;
    private Vector3 handOffset;
    private Transform playerTransform;
    private const float g = 9.7f;
    private Vector3 lastFrameVelocity;

    public void StartSwing(Transform hingePoint)
    {
        enabled = true;
        this.hingePoint = hingePoint;
        var dis =  hand.position - hingePoint.position;
        ropeLength = dis.magnitude;
        
        angularSpeed = Vector3.Dot(Vector3.Cross(dis, Vector3.back).normalized, playerController.Velocity) * Mathf.Rad2Deg / ropeLength;
        if (dis.x > 0)
        {
            angle = Mathf.Asin(dis.y/ropeLength);
        }
        else
        {
            angle = 180*Mathf.Deg2Rad- Mathf.Asin(dis.y/ropeLength);
        }
        
        energyTotal = 0.5f * ropeLength * ropeLength * angularSpeed * angularSpeed;
        rotationEnergy = energyTotal;
        gravitationalPotentialEnergy = (ropeLength - (hingePoint.position.y - hand.position.y)) * g;
        energyTotal += gravitationalPotentialEnergy;
    }
    
    private void Awake()
    {
        playerTransform = playerController.transform;
        handOffset = hand.localPosition;
    }
    private void LateUpdate()
    {
        UpdateRopePhysic();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enabled = false;
            grappleHookController.enabled = true;
            playerController.enabled = true;
            playerController.Velocity = lastFrameVelocity / Time.deltaTime;
            ropeRenderer.ClearRope();
        }
    }

    private void UpdateRopePhysic()
    {
        float rightAxis = Input.GetKey(KeyCode.D) ? 1f : 0f;
        rightAxis = Input.GetKey(KeyCode.A) ? -1f : rightAxis;
        angularSpeed *= (1-(Time.deltaTime * 0.05f));
        angularSpeed += rightAxis * swingAcceleration * Time.deltaTime;
        
        var rotationEnergyNew = 0.5f * ropeLength * ropeLength * angularSpeed * angularSpeed;
        energyTotal += (rotationEnergyNew - rotationEnergy);
        rotationEnergy = rotationEnergyNew;
        
        angle += angularSpeed * Time.deltaTime;
        var handPosition =  hingePoint.position + new Vector3(Mathf.Cos(angle) * ropeLength, Mathf.Sin(angle) * ropeLength, 0);
        Vector3 lastPosition = playerTransform.position;
        playerTransform.position = handPosition - handOffset;
        lastFrameVelocity = playerTransform.position - lastPosition;
        UpdateRope(hand.position);
        var newGPE = (ropeLength - (hingePoint.position.y - hand.position.y)) * g;
        gravitationalPotentialEnergy = newGPE;
        rotationEnergy = energyTotal - newGPE;
        if (rotationEnergy > 0)
        {
            if (angularSpeed > 0)
            {
                angularSpeed = Mathf.Sqrt((rotationEnergy * 2)/(ropeLength*ropeLength));
            }
            else
            {
                angularSpeed = -Mathf.Sqrt((rotationEnergy * 2)/(ropeLength*ropeLength));
            }
        }
        else
        {
            angularSpeed *= -1;
        }

        angularSpeed = Mathf.Clamp(angularSpeed,-maxAngularSpeed, maxAngularSpeed);
    }
    
    private void UpdateRope(Vector3 handPos)
    {
        ropeRenderer.MovePoint(0,handPos);
        ropeRenderer.MovePoint(1,hingePoint.position);
        ropeRenderer.RepaintRope();
    }
}