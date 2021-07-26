using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBase : MonoBehaviour
{
    private Collider2D Collider2D;
    private float timer=7;

    // Start is called before the first frame update
    void Start()
    {
        Collider2D = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Collider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            if (timer <= 0)
                Collider2D.isTrigger = true;
            else timer -= Time.deltaTime;
        }
        else {
            timer = 7;
            Collider2D.isTrigger = false;
        }
    }
}
