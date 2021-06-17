using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseScript : MonoBehaviour
{
    private Animator animator;
    private CapsuleCollider2D collider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    public void Break()
    {
        collider.enabled = false;
        animator.SetTrigger("Broke");
    }
}
