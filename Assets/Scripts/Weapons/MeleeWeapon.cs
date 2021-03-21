using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public int Damage { get; set; }
    public int Durability { get; set; }
    public Transform AttackPosition;
    public float AttackRadius;
    private float TimeBtwnAttack;
    private float StartTimeAttack;
    public static void Action(Vector2 attackPosition, float attackRadius, MeleeWeapon weapon)
    {
    }
    private void Start()
    {
        Damage = 10;
        Durability = 100;
        StartTimeAttack = 1f;
        TimeBtwnAttack = 0;
    }
    void Update()
    {
        if (TimeBtwnAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DoDamage();
                TimeBtwnAttack = StartTimeAttack;

            }
        }
        else 
        {
            TimeBtwnAttack -= Time.deltaTime;
            print("Идет перезарядка");
        }

    }
    public virtual void DoDamage()
    {
            Collider2D[] enemysCollider = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRadius, LayerMask.GetMask("Enemy"));

            if (enemysCollider.Length == 0)
            {
                print("Никого нет в радиусе аттаки");
                return;
            }
;
            foreach (Collider2D enemyCollider in enemysCollider)
            {
                enemyCollider.GetComponent<HP>().TakeDamage(15);
            }
            TimeBtwnAttack = StartTimeAttack; 
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(AttackPosition.position, AttackRadius);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(AttackPosition.position, AttackRadius);
    }
}
