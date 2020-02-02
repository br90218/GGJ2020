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



    public void StartSwing(Transform hingePoint)
    {
        enabled = true;
        this.hingePoint = hingePoint;
        var dis =  hand.position - hingePoint.position;
        ropeLength = dis.magnitude;
        
        angularSpeed = playerController.Velocity.magnitude / ropeLength;
        angularSpeed *= speedToAngular;
        if (dis.x > 0)
        {
            angularSpeed *= -1;
            angle = Mathf.Asin(dis.y/ropeLength);
        }
        else
        {
            angle = 180*Mathf.Deg2Rad- Mathf.Asin(dis.y/ropeLength);
        }
        //Temp
        //angularSpeed = -1;

        //dir.y *= -1;
        

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enabled = false;
            grappleHookController.enabled = true;
            playerController.enabled = true;
            //playerController.Velocity = new Vector3(-angularSpeed * ropeLength * Mathf.Sin(angle),angularSpeed * ropeLength * Mathf.Cos(angle),0) * angularToSpeed;
            playerController.Velocity = new Vector3();
            ropeRenderer.ClearRope();
            return;
        }
        UpdateRopePhysic();
    }

    private void UpdateRopePhysic()
    {
        float rightAxis = Input.GetKey(KeyCode.D) ? 1f : 0f;
        rightAxis = Input.GetKey(KeyCode.A) ? -1f : rightAxis;
        angularSpeed *= (1-(Time.deltaTime * 0.05f));
//        if (angularSpeed > 0)
//        {
//            angularSpeed -= Time.deltaTime * angularDrag;
//            if (angularSpeed < 0)
//            {
//                angularSpeed = 0;
//            }
//        }
//        else
//        {
//            angularSpeed += Time.deltaTime * angularDrag;
//            if (angularSpeed > 0)
//            {
//                angularSpeed = 0;
//            }
//        }
        angularSpeed += rightAxis * swingAcceleration * Time.deltaTime;
        
        
        var rotationEnergyNew = 0.5f * ropeLength * ropeLength * angularSpeed * angularSpeed;
        energyTotal += (rotationEnergyNew - rotationEnergy);
        rotationEnergy = rotationEnergyNew;
        
        angle += angularSpeed * Time.deltaTime;
        hand.position = hingePoint.position + new Vector3(Mathf.Cos(angle) * ropeLength, Mathf.Sin(angle) * ropeLength, 0);
        UpdateRope(hand.position);
        playerTransform.position = hand.position - handOffset;
        var newGPE = (ropeLength - (hingePoint.position.y - hand.position.y)) * g;
        gravitationalPotentialEnergy = newGPE;
        rotationEnergy = energyTotal - newGPE;
        Debug.Log("Energy"+rotationEnergy);
        Debug.Log(angularSpeed);
        if (rotationEnergy > 0)
        {
            if (angularSpeed>0)
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
