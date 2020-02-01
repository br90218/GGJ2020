using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squash : MonoBehaviour
{
    public Animator animator;

    public void PlaySquash() {
        animator.Play("Squash");
    }
    public void PlayStretch() {
        animator.Play("Stretch");
    }
}
