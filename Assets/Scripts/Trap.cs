using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private int _damage = 2;
    private float _currentCD = 0;
    private float _dashCD = 1f;
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (_currentCD <= 0)
        {
            if (collision.tag == "Player")
            {
                collision.GetComponent<HPBar>().TakeDamage(_damage);
                _currentCD = _dashCD;

            }
        }
        else
        {
            _currentCD -= Time.deltaTime;
            print("CD DASH");
        }
    }
}
