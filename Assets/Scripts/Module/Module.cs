using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    private bool _isHold;
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (Input.GetKey(KeyCode.E) && collision.tag == "Player")
        {

            _isHold = true;
            GetComponent<ObjectsMove>().isPicked = true;
            GetComponent<ObjectNameView>().PickUp();
            collision.gameObject.GetComponentInChildren<PlayerController>().Speed = 15f; ;

        }
        if (_isHold)
        {
            transform.position = collision.gameObject.GetComponentInChildren<PlayerController>().HoldP.position;
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }
}
