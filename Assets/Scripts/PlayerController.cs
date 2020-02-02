using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float MaxSpeed = 1f;
    public float Acceleration = 1f;
    public float Decelleration = 1f;
    public float JumpSpeed = 1f;
    public float JumpDeceleration = 1f;
    public float Gravity;
    public float AirSteerRatio = 0.5f;
    public ParticleSystem dustParticle;

    public Squash Squasher;
    public Vector3 Velocity;
    public GroundDetector ground;
    public LayerMask groundLayer;

    public void Update() {
        Jump();
        HorizontalMovement();
        transform.position += Velocity;
    }

    protected void Jump() {
        var dropRCH = Physics2D.CircleCast(transform.position, 0.25f, Vector3.down, 20f,groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && ground.GroundContact) {
            Velocity.y += JumpSpeed;
            ground.GroundContact = false;
            ground.LastJumpTime = Time.time;
            Squasher.PlayStretch();
        }

        if (Velocity.y > 0f && !Input.GetKey(KeyCode.Space)) {
            Velocity.y -= JumpDeceleration * Time.deltaTime;
        }

        if (dropRCH.collider == null || dropRCH.distance > 0.43f) {
            Velocity.y -= Gravity * Time.deltaTime;
        }
        else if (Velocity.y < 0f) {
            float maxDrop = dropRCH.distance - 0.43f;
            if (Velocity.y < -maxDrop) {
                transform.position += Vector3.down * maxDrop;
                if (Mathf.Abs(Velocity.y) > Mathf.Abs(JumpSpeed) / 2f) {
                    Squasher.PlaySquash();
                    dustParticle.Play();
                }
                Velocity.y = 0f;
            }
        }
    }

    protected void HorizontalMovement() {
        float rightAxis = Input.GetKey(KeyCode.D) ? 1f : 0f;
        rightAxis = Input.GetKey(KeyCode.A) ? -1f : rightAxis;
        float acceleration = Acceleration;
        bool accelerating = true;
        float xDir = Velocity.x == 0f ? 0f : Velocity.x / Mathf.Abs(Velocity.x);
        var forwardRCH = Physics2D.CircleCast(transform.position, 0.2f, Vector3.right * xDir, 2f,groundLayer);
        if (xDir != rightAxis && xDir != 0f) {
            acceleration = Decelleration;
            accelerating = false;
            if (Velocity.x != 0f) {
                rightAxis = -xDir;
            }
        }
        if (!ground.GroundContact) {
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