using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    public Transform Firepoint;
    public GameObject Ammo;
    public bool IsShooting;

    private float reloadTime; 
    private float shootKD = 0f;
    private WeaponStats weaponStats;

    private void Start()
    {
        weaponStats = gameObject.GetComponentInChildren<WeaponStats>();
        reloadTime = weaponStats.ReloadTime;
    }

    void Update()
    {
        //Firepoint.rotation = gameObject.transform.rotation;
        if (IsShooting)
            if (shootKD <= 0)
            {
                Shoot();
                shootKD = reloadTime;
            }
            else shootKD -= Time.deltaTime;
    }

    private void Shoot()
    {   
        var bullet = Instantiate(Ammo, Firepoint.position, Firepoint.rotation).GetComponent<Bullet>();
        bullet.Owner = gameObject;
        bullet.Weapon = weaponStats;
    }
}
