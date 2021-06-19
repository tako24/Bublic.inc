using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed;
    public GameObject Owner;
    public WeaponStats Weapon;

    void Start()
    {
        var shotDirection = Weapon.gameObject.GetComponent<ShootSystem>().Firepoint.transform.position 
            - gameObject.transform.position;
        GetComponent<Rigidbody2D>().velocity = shotDirection * Speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Room") || collision.CompareTag("Exit"))
        {
            var hpBar = collision.gameObject.GetComponent<HPBar>();

            if (hpBar != null)
                hpBar.TakeDamage(Weapon.Damage);

            Destroy(gameObject);
        }
    }
}
