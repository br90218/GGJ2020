using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Rigidbody2D rb;

    public void Update() {
        rb.AddForce(Vector2.right * Input.GetAxis("Horizontal"));
    }
}