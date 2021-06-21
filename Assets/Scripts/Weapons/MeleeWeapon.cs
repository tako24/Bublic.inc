using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public int Damage;
    public float AttackCooldown;
    private float KD;

    public Collider2D AttackArea;
    public SpriteRenderer WeaponSprite;

    public AudioClip AttackSound;

    public bool IsPicked;
    public bool IsInRange;

    void Start()
    {
        AttackArea = GetComponent<CompositeCollider2D>();
        WeaponSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (IsPicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (KD <= 0)
                {
                    WeaponSprite.color = Color.white;
                    DoDamage();
                    gameObject.GetComponent<AudioSource>().PlayOneShot(AttackSound);
                    KD = AttackCooldown;
                }
            }
            else if (KD > 0)
            {
                KD -= Time.deltaTime;
                if (KD <= 0)
                    StartCoroutine(Blink());
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && IsInRange && !IsPicked)
            PickUp();
    }

    public IEnumerator Blink()
    {
        WeaponSprite.color = new Color(0.18f, 0.3f, 0.3f);
        yield return new WaitForSeconds(0.25f);
        WeaponSprite.color = Color.white;
    }

    public void PickUp()
    {
        IsPicked = true;

        GetComponent<ObjectsMove>().enabled = false;
        GetComponent<ObjectNameView>().enabled = false;

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<PolygonCollider2D>().enabled = true;

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        GameController.Inventory.CollectItem(gameObject);
    }

    public void DoDamage()
    {
        var anim = GetComponent<Animator>();
        anim.SetTrigger("Attacked");

        var enemysCollider = new List<Collider2D>();
        var filter = new ContactFilter2D();
        Physics2D.OverlapCollider(AttackArea, filter, enemysCollider);

        if (enemysCollider.Count == 0)
        {
            print("Никого нет в радиусе аттаки");
            return;
        }

        foreach (Collider2D enemyCollider in enemysCollider)
        {
            if (!enemyCollider.CompareTag("Enemy")
                && !enemyCollider.CompareTag("Destroyable")) continue;
            enemyCollider.GetComponent<HP>().TakeDamage(Damage + GameController.DamageBonus);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPicked) IsInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsPicked) IsInRange = false;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(AttackPosition.position, AttackRadius);
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(AttackPosition.position, AttackRadius);
    //}
}
