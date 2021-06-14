using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 0.0001f;
    public GameObject Owner;
    public WeaponStats Weapon;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.forward * Speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.gameObject.GetHashCode() != Owner.GetHashCode()
            && !collision.gameObject.CompareTag("Trap") 
            && (!collision.gameObject.CompareTag("Exit") 
            || !collision.gameObject.GetComponentInParent<RoomProperties>().IsCleared)
            && !collision.gameObject.CompareTag("ammo"))
        {
            var hp = collision.gameObject.GetComponent<HP>();

            if (hp != null)
                hp.TakeDamage(Weapon.Damage);

            var hpBar = collision.gameObject.GetComponent<HPBar>();

            if (hpBar != null)
                hpBar.TakeDamage(Weapon.Damage);

            Destroy(gameObject);
        }
    }
}
