﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinchController : MonoBehaviour
{
    public SpringJoint2D spring;
    public float interactionSqrDistance;
    public float towSpeed = 1f;
    public Animator animator;
    private bool towing;
    void Update()
    {
        if (!towing && Input.GetKeyDown(KeyCode.E)) {
            var controller = GameObject.FindObjectOfType<PlayerController>();
            var distance = (transform.position - controller.transform.position).sqrMagnitude;
            var playerSpring = controller.GetComponent<SpringJoint2D>();

            if (distance < interactionSqrDistance) {
                spring.connectedBody = playerSpring.connectedBody;
                playerSpring.connectedBody = null;
                towing = true;
                animator.SetBool("cranking", true);
            }
        }
        if (towing) {
            if (Input.GetKeyUp(KeyCode.E)) {
                spring.connectedBody = null;
                towing = false;
                animator.SetBool("cranking", false);
                return;
            }
            spring.distance -= towSpeed * Time.deltaTime;
        }
    }
}
