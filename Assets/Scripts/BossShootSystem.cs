using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossShootSystem : MonoBehaviour
{
    public int ShootCount;
    private bool isShooting=false;
    private ShootSystem[] shootSystems;
    // Start is called before the first frame update
    void Start()
    {
        shootSystems = GetComponentsInChildren<ShootSystem>();
    }
    public bool IsShooting() =>isShooting;
    public void StartShooting()
    {
        isShooting = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (shootSystems.Length == 0)
        {
            shootSystems = GetComponentsInChildren<ShootSystem>();
        }
        if (isShooting)
        {
            if (shootSystems.Where(x => x.shootCount <= ShootCount).Count() <= 0)
            {
                isShooting = false;
                foreach (var e in shootSystems)
                {
                    e.shootCount = 0;
                }
            }
        }
        
    }
}
