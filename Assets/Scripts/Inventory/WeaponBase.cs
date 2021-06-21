using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public void DisplayWeapon(InventoryItem weapon)
    {
        weapon.mainObject.SetActive(true);
        GameController.Player.GetComponent<PlayerController>().Weapon = weapon.mainObject;
    }
}
