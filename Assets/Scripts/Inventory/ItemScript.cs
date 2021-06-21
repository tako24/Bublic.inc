using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField] public InventoryItem InventoryItem;
    public int id;
    public GameObject mainObject;
    public Sprite icon;
    public bool isModule = false;
    public bool isWeapon = false;
    public bool isHeal = false;
    void Start()
    {
        InventoryItem = new InventoryItem(gameObject, id,icon, isModule, isWeapon, isHeal);
    }
}
