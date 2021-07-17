using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestController : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            gameObject.GetComponentInParent<MoskitoController>().TheeToFour();
                
    }
}
