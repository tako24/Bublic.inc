using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
	public Transform attackPosition;
	public float attackRadius;
	public MeleeWeapon currentMeleeWeapon;
    private void Start()
    {
	}
    public  static void Action(Vector2 attackPosition, float attackRadius, MeleeWeapon weapon)
	{
		Collider2D[] enemysCollider = Physics2D.OverlapCircleAll(attackPosition, attackRadius, LayerMask.GetMask("Enemy"));

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
	}
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{ 
			Action(attackPosition.position, attackRadius,currentMeleeWeapon);
		}
	}
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(attackPosition.position, attackRadius);
    }
}
