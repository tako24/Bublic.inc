using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public GameObject owner;
    public weaponStats weapon;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetHashCode()!=owner.GetHashCode()&&collision.gameObject.tag!="Trap" && (collision.gameObject.tag != "Exit"||!collision.gameObject.GetComponentInParent<RoomProperties>().IsCleared))
        {
            var hp = collision.gameObject.GetComponent<HP>();
            if (hp != null)
                hp.TakeDamage(weapon.Damage);
            Destroy(gameObject);
        }
    }
}
