using UnityEngine;

public class ShootSystem : MonoBehaviour
{
    public Transform Firepoint;
    public GameObject Ammo;
    public bool IsShooting;
    public bool IsBoss;
    public GameObject boss;
    public WeaponStats weaponStats;
    public int shootCount = 0;

    private float reloadTime; 
    private float shootKD = 0f;

    private void Start()
    {
        weaponStats = gameObject.GetComponentInChildren<WeaponStats>();
        reloadTime = weaponStats.ReloadTime;
        if (weaponStats == null)
            weaponStats = gameObject.GetComponentInChildren<WeaponStats>();
    }

    void Update()
    {
        //Firepoint.rotation = gameObject.transform.rotation;
        if (IsShooting)
            if (shootKD <= 0)
            {
                Shoot();
                shootCount++;
                shootKD = reloadTime;
            }
            else shootKD -= Time.deltaTime;
    }

    public void InstantReload()
    {
        shootKD = 0;
    }

    private void Shoot()
    {
        {
            if (!IsBoss)
            {
                var bullet = Instantiate(Ammo, gameObject.transform.position, Firepoint.rotation).GetComponent<Bullet>();
                bullet.Owner = gameObject;
                bullet.Weapon = weaponStats;
            }
            else
            {
                var bullet = Instantiate(Ammo, gameObject.transform.position, Firepoint.rotation).GetComponent<Laser>();
                bullet.Owner = gameObject;
                bullet.Weapon = weaponStats;
                bullet.boss = boss;
            }
        }
    }
}
