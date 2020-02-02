using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public SpriteRenderer playerSpriteRenderer;
    public float walkThreshold = 0.1f;
    

    private Vector3 lastPosition;

    private void LateUpdate()
    {
        var speed = (lastPosition - player.position) / Time.deltaTime;
        if (speed.x> walkThreshold || speed.x < -walkThreshold)
        {
            animator.SetBool("walking",true);
        }
        else
        {
            animator.SetBool("walking",false);
        }

        if (speed.x > 0)
        {
            playerSpriteRenderer.flipX = true;
        }
        else if (speed.x < 0)
        {
            playerSpriteRenderer.flipX = false;
        }

        lastPosition = player.position;
    }
}
