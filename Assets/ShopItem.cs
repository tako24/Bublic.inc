using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public bool InRange;
    public int Price;
    public Text PriceText;
    public List<GameObject> ItemsToSell;
    public GameObject SoldItem;
    public Text CoinsText;

    private void Start()
    {
        SoldItem = Instantiate(ItemsToSell[Random.Range(0, ItemsToSell.Count)], 
            transform.position, Quaternion.identity, transform);

        if (SoldItem.GetComponent<ObjectNameView>() != null)
            SoldItem.GetComponent<ObjectNameView>().enabled = false;
        if (SoldItem.GetComponentInChildren<CircleCollider2D>() != null)
            SoldItem.GetComponentInChildren<CircleCollider2D>().enabled = false;
        if (SoldItem.GetComponent<CircleCollider2D>() != null)
            SoldItem.GetComponent<CircleCollider2D>().enabled = false;
        if (SoldItem.GetComponent<BoxCollider2D>() != null)
            SoldItem.GetComponent<BoxCollider2D>().enabled = false;

        CoinsText = GameObject.Find("Coins").GetComponentInChildren<Text>();
        PriceText = GetComponentInChildren<Text>();
        PriceText.text = Price.ToString();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InRange = true;
            PriceText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InRange = false;
            PriceText.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && InRange)
            BuyItem();
    }

    public void BuyItem()
    {
        if (CoinsText.GetComponent<CoinScore>().CoinCount >= Price)
        {
            CoinsText.GetComponent<CoinScore>().SpendCoins(Price);
            GameController.Inventory.CollectItem(SoldItem);
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(FlashCoins());
    }

    IEnumerator FlashCoins()
    {
        CoinsText.color = Color.red;
        yield return new WaitForSeconds(1);
        CoinsText.color = Color.white;
    }
}
