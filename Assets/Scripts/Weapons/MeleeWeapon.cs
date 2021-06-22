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
    public float DistanceOffset;
    public float AngleOffset;

    public AudioClip AttackSound;

    public bool IsPicked;
    public bool IsInRange;

    public Animator Animator;

    void Start()
    {
        AttackArea = GetComponent<Collider2D>();
        WeaponSprite = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsPicked && Time.timeScale > 0)
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
            if (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
                AttackArea.enabled = true;
            else
                AttackArea.enabled = false;
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
        //GetComponent<Collider2D>().enabled = true;

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        GameController.Inventory.CollectItem(gameObject);
    }

    public void DoDamage()
    {
        Animator.SetTrigger("Attacked");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPicked && collision.CompareTag("Player")) 
            IsInRange = true;
        else if (collision.CompareTag("Enemy") || collision.CompareTag("Destroyable"))
            collision.GetComponent<HP>().TakeDamage(Damage + GameController.DamageBonus);
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
