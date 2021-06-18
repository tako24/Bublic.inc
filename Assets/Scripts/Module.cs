using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    private bool _isHold = false;
    GameObject _player;
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
            collision.gameObject.GetComponentInChildren<PlayerController>().Speed = 15f;
            _player = collision.gameObject;

        }
        if (_isHold)
        {
            transform.position = _player.GetComponent<PlayerController>()._holdP.position;

        }
        //else
        //    collision.gameObject.GetComponentInChildren<PlayerController>().Speed = 5f;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }
}