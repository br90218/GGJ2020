using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public Transform LeftFoot;
    public Transform RightFoot;
    public bool GroundContact;
    public bool LeftDangle;
    public bool RightDangle;
    public LayerMask groundLayer;

    protected float LastGroundContactRight;
    protected float LastGroundContactLeft;
    public float LastJumpTime;
    public float JumpGraceTime = 0.02f;
    public Transform Body;

    public void Update() {
        var leftRCH = Physics2D.Raycast(LeftFoot.position, Vector3.down, 0.15f,groundLayer);
        var rightRCH = Physics2D.Raycast(RightFoot.position, Vector3.down, 0.15f,groundLayer);

        LeftDangle = leftRCH.collider == null;
        RightDangle = rightRCH.collider == null;

        if (!LeftDangle) {
            LastGroundContactLeft = Time.time;
        }
        if (!RightDangle) {
            LastGroundContactRight = Time.time;
        }
        
        var thresholdTime = Time.time - JumpGraceTime;
        GroundContact = (LastGroundContactLeft > thresholdTime || LastGroundContactRight > thresholdTime) && LastJumpTime < thresholdTime;
        
        if (LeftDangle && !RightDangle) {
            Body.transform.rotation = Quaternion.Euler(0f, 0f, 20f);
        }
        else if (!LeftDangle && RightDangle) {
            Body.transform.rotation = Quaternion.Euler(0f, 0f, -20f);
        }
        else {
            Body.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
