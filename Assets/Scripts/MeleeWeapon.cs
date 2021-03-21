using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private void Start()
    {
        Damage = 10;
    }
    public int Damage { get; set; }

    // Update is called once per frame
}
