using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SFXController : MonoBehaviour
{
    public Transform player;
    public float walkThreshold = 0.4f;
    public GroundDetector groundDetector;
    public SwingStrafeController swingStrafeController;
    private Vector3 lastPosition;
    private bool grounded;
    [FormerlySerializedAs("inLowPoint")] [SerializeField]
    private bool InHighPoint = true;
    private void LateUpdate()
    {
        var speed = (lastPosition - player.position) / Time.deltaTime;
        if (speed.x> walkThreshold || speed.x < -walkThreshold)
        {
            if (!AudioManager.instance.isPlaying("Walk"))
            {
                AudioManager.instance.Play("Walk");
            }
        }
        else
        {
            Debug.Log("Stop");
            AudioManager.instance.Stop("Walk");
        }
        lastPosition = player.position;

        if (groundDetector.GroundContact != grounded && groundDetector.GroundContact)
        {
            AudioManager.instance.Play("LandOn");
        }
        grounded = groundDetector.GroundContact;

        if (swingStrafeController.isActiveAndEnabled && !grounded)
        {
            var angle = swingStrafeController.angle % (Mathf.Deg2Rad * 360);
            if (angle<0)
            {
                angle += Mathf.Deg2Rad * 360;
            }
//            Debug.Log((Mathf.Deg2Rad * 360));
//            Debug.Log(angle);
            if (InHighPoint)
            {
                if (angle > 4.4 && angle < 5)
                {
                    AudioManager.instance.Play("Swing");
                    InHighPoint = false;
                }
            }
            else
            {
                if (angle < 4.4 || angle > 5f)
                {
                    
                    InHighPoint = true;
                }
            }
        }
    }
}
