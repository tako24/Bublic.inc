using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleSlot : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null&&eventData.pointerDrag.GetComponent<InventoryItemScript>().inventoryItem.isModule && GameObject.Find("Inventory").GetComponent<InventoryScript>().isMoving())
        {
            var cord = int.Parse(gameObject.name);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<InventoryItemScript>().SavePosition();
            GameObject.Find("Inventory").GetComponent<InventoryScript>().MoveToModuleSlot(cord);
        }
    }
}
