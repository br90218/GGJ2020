using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public Transform player;
    public float walkThreshold = 0.4f;
    public GroundDetector groundDetector;
    private Vector3 lastPosition;
    private bool grounded;

    private void LateUpdate()
    {
        var speed = (lastPosition - player.position) / Time.deltaTime;
        if (speed.x> walkThreshold || speed.x < -walkThreshold)
        {
            AudioManager.instance.Play("walk");
        }
        else
        {
            AudioManager.instance.Stop("walk");
        }

        if (groundDetector.GroundContact != grounded && groundDetector.GroundContact)
        {
            AudioManager.instance.Play("LandOn");
        }
        grounded = groundDetector.GroundContact;

    }
}
