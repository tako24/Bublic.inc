using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    public int Damage { get; set; }
    public int Durability { get; set; }
    public float TimeBtwnAttack { get; set; }
    public float StartTimeAttack { get; set; }
    void DoDamage();

}
