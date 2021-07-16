using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTriger : MonoBehaviour
{
    private Rotator rotator;
    private void Start()
    {
        rotator = gameObject.transform.parent.gameObject.GetComponent<Rotator>();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            rotator.StartRotate();
    }
}
