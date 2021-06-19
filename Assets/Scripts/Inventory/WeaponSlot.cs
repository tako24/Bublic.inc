using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop");
        if (eventData.pointerDrag != null&&eventData.pointerDrag.GetComponent<InventoryItemScript>().inventoryItem.isWeapon && GameObject.Find("Inventory").GetComponent<InventoryScript>().isMoving())
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<InventoryItemScript>().SavePosition();
            GameObject.Find("Inventory").GetComponent<InventoryScript>().MoveToWeaponSlot();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
