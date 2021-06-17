using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public int Damage;
    public float AttackCooldown;
    private float KD = 0f;

    public EdgeCollider2D AttackArea;

    void Start()
    {
        AttackArea = GetComponent<EdgeCollider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (KD <= 0)
            {
                DoDamage();
                KD = AttackCooldown / 100;
            }

            else
                KD -= Time.deltaTime;
        }
    }

    public void PickUp()
    {
        GetComponent<ObjectNameView>().enabled = false;
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
            enemyCollider.GetComponent<HP>().TakeDamage(Damage);
        }
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
