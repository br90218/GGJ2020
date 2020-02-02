using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public SwingStrafeController swingStrafeController;
    public float edgeDetectionDistance = 0.1f;
    private Vector2 boxSize = new Vector2(0.8f,1.3f);
    private Transform player;
    private void Awake()
    {
        player = GetComponent<Transform>();
        
    }

    private void Update()
    {
        if (Physics2D.BoxCast(player.position, boxSize, 0, Vector2.right, edgeDetectionDistance))
        {
            Debug.Log("Wall");
            swingStrafeController.edgeDetection = 1;
        }
        else if (Physics2D.BoxCast(player.position, boxSize, 0, Vector2.left, edgeDetectionDistance))
        {
            
            Debug.Log("Wall");
            swingStrafeController.edgeDetection = 1;
        }
        else
        {
            swingStrafeController.edgeDetection = 0;
        }
    }
}
