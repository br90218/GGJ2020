using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float MaxSpeed = 1f;
    public float Acceleration = 1f;
    public float Decelleration = 1f;
    public float JumpSpeed = 1f;
    public float JumpDeceleration = 1f;
    public Transform Body;
    public float JumpGraceTime = 0.02f;
    public float Gravity;
    public float AirSteerRatio = 0.5f;
    public ParticleSystem particleSystem;

    public Transform LeftFoot;
    public Transform RightFoot;
    public Squash Squasher;

    protected float LastGroundContactRight;
    protected float LastGroundContactLeft;
    protected float LastJumpTime;
    protected Vector3 Velocity;
    protected bool groundContact;

    public void Update() {
        transform.position += Velocity / 2f;
        Jump();
        HorizontalMovement();
        transform.position += Velocity / 2f;
    }

    protected void Jump() {
        var leftRCH = Physics2D.Raycast(LeftFoot.position, Vector3.down, 0.15f);
        var rightRCH = Physics2D.Raycast(RightFoot.position, Vector3.down, 0.15f);
        var dropRCH = Physics2D.CircleCast(transform.position, 0.25f, Vector3.down, 20f);

        bool leftDangle = leftRCH.collider == null;
        bool rightDangle = rightRCH.collider == null;

        if (!leftDangle) {
            LastGroundContactLeft = Time.time;
        }
        if (!rightDangle) {
            LastGroundContactRight = Time.time;
        }
        
        var thresholdTime = Time.time - JumpGraceTime;
        groundContact = (LastGroundContactLeft > thresholdTime || LastGroundContactRight > thresholdTime) && LastJumpTime < thresholdTime;

        if (leftDangle && !rightDangle) {
            Body.transform.rotation = Quaternion.Euler(0f, 0f, 20f);
        }
        else if (!leftDangle && rightDangle) {
            Body.transform.rotation = Quaternion.Euler(0f, 0f, -20f);
        }
        else {
            Body.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && groundContact) {
            Velocity.y += JumpSpeed;
            groundContact = false;
            LastJumpTime = Time.time;
            Squasher.PlayStretch();
        }

        if (Velocity.y > 0f && !Input.GetKey(KeyCode.Space)) {
            Velocity.y -= JumpDeceleration * Time.deltaTime;
        }

        if (leftDangle && rightDangle) {
            Velocity.y -= Gravity * Time.deltaTime;
        }
        else {
            if (Velocity.y < 0f) {
                float maxDrop = dropRCH.distance - 0.23f;
                if (Velocity.y < maxDrop && Mathf.Abs(Velocity.y) > Mathf.Abs(maxDrop)) {
                    transform.position += Vector3.down * maxDrop;
                    if (Mathf.Abs(Velocity.y) > Mathf.Abs(JumpSpeed) / 2f) {
                        Squasher.PlaySquash();
                        particleSystem.Play();
                    }
                    Velocity.y = 0f;
                }
            }
        }
    }

    protected void HorizontalMovement() {
        float rightAxis = Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
        rightAxis = Input.GetKey(KeyCode.LeftArrow) ? -1f : rightAxis;
        float acceleration = Acceleration;
        bool accelerating = true;
        float xDir = Velocity.x == 0f ? 0f : Velocity.x / Mathf.Abs(Velocity.x);
        var forwardRCH = Physics2D.CircleCast(transform.position, 0.2f, Vector3.right * xDir, 2f);
        if (xDir != rightAxis && xDir != 0f) {
            acceleration = Decelleration;
            accelerating = false;
            if (Velocity.x != 0f) {
                rightAxis = -xDir;
            }
        }
        if (!groundContact) {
            acceleration *= AirSteerRatio;
        }
        var change = Vector3.right * rightAxis * Time.deltaTime * acceleration;
        Velocity += change;
        if (!accelerating && Velocity.sqrMagnitude < change.sqrMagnitude) {
            Velocity = Vector3.zero;
        }
        if (Velocity.sqrMagnitude > MaxSpeed * MaxSpeed) {
            Velocity = Velocity.normalized * MaxSpeed;
        }
        float maxMove = forwardRCH.distance - 0.1f;
        if (Mathf.Abs(Velocity.y) > maxMove && forwardRCH.collider != null) {
            transform.position += Vector3.right * xDir * maxMove;
            Velocity.x = 0f;
        }
    }
}