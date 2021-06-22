using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealScript : MonoBehaviour
{
    public int HealAmount;
    public bool IsInRange;
    public bool IsPicked;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            IsInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            IsInRange = false;
    }

    private void Update()
    {
        if (!IsPicked && Input.GetKeyDown(KeyCode.E) && IsInRange)
            CollectHeal();
    }

    public void CollectHeal()
    {
        GameController.Inventory.CollectItem(gameObject);
        try
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<ObjectsMove>().enabled = false;
            GetComponent<ObjectNameView>().enabled = false;
        }
        catch { }
        gameObject.SetActive(false);
    }

    public void Heal()
    {
        GameController.Player.GetComponent<HPBar>().Heal(HealAmount);
    }
}
