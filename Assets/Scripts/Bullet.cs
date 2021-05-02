using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public GameObject owner;
    public WeaponStats weapon;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.forward * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.GetHashCode()!=owner.GetHashCode()&& !collision.gameObject.CompareTag("Trap") && (!collision.gameObject.CompareTag("Exit") || !collision.gameObject.GetComponentInParent<RoomProperties>().IsCleared)&& !collision.gameObject.CompareTag("ammo"))
        {
            var hp = collision.gameObject.GetComponent<HP>();
            if (hp != null)
                hp.TakeDamage(weapon.Damage);
            var hpBar = collision.gameObject.GetComponent<HPBar>();
            if (hpBar != null)
                hpBar.TakeDamage(weapon.Damage);
            Destroy(gameObject);
        }
    }
}
