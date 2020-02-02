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
    public float ropeRetractSpeed = 1f;
    public LayerMask obstacle;
    public int edgeDetection;
    
    [Header("Controller and stuff")]
    public RopeRenderer ropeRenderer;
    public Transform hand;
    public GrappleHookController grappleHookController;
    public PlayerController playerController;
    public GroundDetector groundDetector;
    
    
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
    private Vector2 boxSize = new Vector2(0.8f,1.3f);



    public void StartSwing(Transform hingePoint)
    {
        enabled = true;
        this.hingePoint = hingePoint;
        var dis =  hand.position - hingePoint.position;
        ropeLength = dis.magnitude;
        
        angularSpeed = playerController.Velocity.magnitude / ropeLength;
        angularSpeed *= speedToAngular;
        if (playerController.Velocity.x < 0)
        {
            angularSpeed *= -1;
        }
        if (dis.x > 0)
        {
            //angularSpeed *= -1;
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
        RopeRetraction();
        if (groundDetector.GroundContact)
        {
            playerController.enabled = true;
        }
        else
        {
            UpdateRopePhysic();
            playerController.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enabled = false;
            if (!groundDetector.GroundContact)
            {
                grappleHookController.enabled = true;
                playerController.enabled = true;
                playerController.Velocity = new Vector3(-angularSpeed * ropeLength * Mathf.Sin(angle),angularSpeed * ropeLength * Mathf.Cos(angle),0) * angularToSpeed;
            }
            grappleHookController.enabled = true;
            playerController.enabled = true;
            playerController.Velocity = new Vector3(-angularSpeed * ropeLength * Mathf.Sin(angle),angularSpeed * ropeLength * Mathf.Cos(angle),0) * angularToSpeed;
            //Debug.Log(playerController.Velocity);
            //playerController.Velocity = new Vector3();
            ropeRenderer.ClearRope();
            return;
        }
        
        UpdateRope(hand.position);
    }

    private void RopeRetraction()
    {
        if (groundDetector.GroundContact)
        {
            var dis =  hand.position - hingePoint.position;
            ropeLength = dis.magnitude;
            if (playerController.Velocity.x < 0)
            {
                angularSpeed *= -1;
            }
            if (dis.x > 0)
            {
                //angularSpeed *= -1;
                angle = Mathf.Asin(dis.y/ropeLength);
            }
            else
            {
                angle = 180*Mathf.Deg2Rad- Mathf.Asin(dis.y/ropeLength);
            }
            if (Input.GetKey(KeyCode.E))
            {
                var newLength = ropeLength - ropeRetractSpeed * Time.deltaTime;
                if (newLength <= 1f)
                {
                    newLength = 1f;
                }
                ropeLength = newLength;
                hand.position = hingePoint.position + new Vector3(Mathf.Cos(angle) * ropeLength, Mathf.Sin(angle) * ropeLength, 0);
                playerTransform.position = hand.position - handOffset;
                
            }
            return;
        }
        if (Input.GetKey(KeyCode.E))
        {
            var newLength = ropeLength - ropeRetractSpeed * Time.deltaTime;
            if (newLength <= 1f)
            {
                newLength = 1f;
            }
            angularSpeed = angularSpeed * newLength / ropeLength;
            ropeLength = newLength;
        }
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

        RaycastHit hit;
        var newAngle = angle + angularSpeed * Time.deltaTime;
        var newPosition = hingePoint.position + new Vector3(Mathf.Cos(newAngle) * ropeLength, Mathf.Sin(newAngle) * ropeLength, 0);
        newPosition -= handOffset;
        Vector2 travelDistance = newPosition - playerTransform.position;
        
        if (Physics2D.BoxCast(newPosition, boxSize, 0, Vector2.right, travelDistance.magnitude))
        {
            Debug.Log("Wall");
            angularSpeed *= -1;
        }
        else if (Physics2D.BoxCast(newPosition, boxSize, 0, Vector2.left, travelDistance.magnitude))
        {
            
            Debug.Log("Wall");
            angularSpeed *= -1;
            
        }

        //Debug.Log(travelDistance.magnitude);
        if (edgeDetection > 0)
        {
            //Debug.Log(travelDistance.magnitude);
            Debug.Log("Hit Wall");
            angularSpeed *= -1;
        }



        var rotationEnergyNew = 0.5f * ropeLength * ropeLength * angularSpeed * angularSpeed;
        energyTotal += (rotationEnergyNew - rotationEnergy);
        rotationEnergy = rotationEnergyNew;
        
        angle += angularSpeed * Time.deltaTime;
        hand.position = hingePoint.position + new Vector3(Mathf.Cos(angle) * ropeLength, Mathf.Sin(angle) * ropeLength, 0);
        playerTransform.position = hand.position - handOffset;
        var newGPE = (ropeLength - (hingePoint.position.y - hand.position.y)) * g;
        gravitationalPotentialEnergy = newGPE;
        rotationEnergy = energyTotal - newGPE;
        //Debug.Log("Energy"+rotationEnergy);
        //Debug.Log(angularSpeed);
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