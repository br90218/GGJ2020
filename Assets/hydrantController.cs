using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hydrantController : MonoBehaviour
{
    public Rigidbody2D top;
    public float errorRange;
    public void Update() {
        if (Quaternion.Angle(top.transform.rotation, Quaternion.identity) < errorRange) {
            top.transform.rotation = Quaternion.identity;
            top.bodyType = RigidbodyType2D.Static;
            LevelManager.Instance.score ++;
            enabled = false;
        }
    }
}
