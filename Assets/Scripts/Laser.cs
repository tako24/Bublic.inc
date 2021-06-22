using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float Speed;
    public GameObject Owner;
    public WeaponStats Weapon;
    public GameObject boss;
    public float LiveTime=10;
    private float totalLiveTime = 0;

    void Start()
    {
        var shotDirection = Weapon.gameObject.GetComponent<ShootSystem>().Firepoint.transform.position
            - boss.transform.position;
        GetComponent<Rigidbody2D>().velocity = shotDirection * Speed;
    }
    private void Update()
    {
        totalLiveTime += Time.deltaTime;
        if (totalLiveTime >= LiveTime)
        {
            Destroy(gameObject);
        }
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
