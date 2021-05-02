using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firepoint;
    public GameObject ammo;
    public bool IsShooting=false;
    private float reloadTime; 
    private float shootKD=0;
    private WeaponStats weaponStats;
    private void Start()
    {
          weaponStats= gameObject.GetComponentInChildren<WeaponStats>();
        reloadTime = weaponStats.ReloadTime;
    }

    void Update()
    {
        firepoint.rotation = gameObject.transform.rotation;
        if (IsShooting)
            if (shootKD <= 0)
            {
                Shoot();
                shootKD = reloadTime;
            }
            else shootKD -= Time.deltaTime;
    }

    private void Shoot()
    {   var bullet=Instantiate(ammo, firepoint.position, firepoint.rotation).GetComponent<Bullet>();
        bullet.owner = gameObject;
        bullet.weapon = weaponStats;
    }
}
