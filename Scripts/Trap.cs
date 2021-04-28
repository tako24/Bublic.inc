using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int _damage = 35;
    [SerializeField] private float _currentCD = 0;
    [SerializeField] private float _trapCD = 1f;
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
                _currentCD = _trapCD;

            }
        }
        else
        {
            _currentCD -= Time.deltaTime;
        }
    }
}
