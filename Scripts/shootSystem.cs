using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firepoint;
    public GameObject ammo;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {   var bullet=Instantiate(ammo, firepoint.position, firepoint.rotation).GetComponent<Bullet>();
        bullet.owner = gameObject;
        bullet.weapon = gameObject.GetComponentInChildren<weaponStats>();
    }
}
