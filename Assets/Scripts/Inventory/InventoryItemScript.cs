using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static InventoryScript;

public class InventoryItemScript : MonoBehaviour,IPointerDownHandler,IBeginDragHandler,IEndDragHandler,IDragHandler,IDropHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 oldPosition;
    public InventoryItem inventoryItem;
    [SerializeField] private Canvas canvas;
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        GameObject.Find("Inventory").GetComponent<InventoryScript>().StartMoveItem(inventoryItem);
        canvasGroup.alpha = .6f;
    }
    public void SavePosition()
    {
        var newPosition = rectTransform.anchoredPosition;
        oldPosition = new Vector2(newPosition.x, newPosition.y);
    }
    public void ReturnPosition()
    {
        rectTransform.anchoredPosition = oldPosition;
    }
    public void OnDrag(PointerEventData eventData)
    {

        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        ReturnPosition();
        GameObject.Find("Inventory").GetComponent<InventoryScript>().EndMoveItem();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        SavePosition();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
