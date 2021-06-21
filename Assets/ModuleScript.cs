using UnityEngine;

public class ModuleScript : MonoBehaviour
{
    public IModuleEffect Effect;
    public bool IsInRange;
    public bool IsPicked;

    private void Start()
    {
        Effect = GetComponentInChildren<IModuleEffect>();
    }

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
        {
            GameController.Inventory.CollectItem(gameObject);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<ObjectsMove>().enabled = false;
            GetComponent<ObjectNameView>().enabled = false;
            gameObject.SetActive(false);
        }
    }

    public void Activate(bool activate)
    {
        if (activate)
        {
            gameObject.SetActive(true);
            Effect.ActivateEffect(true);
        }
        else
        {
            gameObject.SetActive(false);
            Effect.ActivateEffect(false);
        }
    }
}
