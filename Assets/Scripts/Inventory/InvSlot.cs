using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvSlot : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && GameObject.Find("Inventory").GetComponent<InventoryScript>().isMoving())
        {
            var cords = gameObject.name.Split(' ').Select(x => int.Parse(x)).ToArray();
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<InventoryItemScript>().SavePosition();
            GameObject.Find("Inventory").GetComponent<InventoryScript>().MoveToInv(cords[0], cords[1]);
        }
    }
}
