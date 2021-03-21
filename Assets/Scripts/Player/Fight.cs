using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
	public Transform attackPosition;
	public float attackRadius;
	public MeleeWeapon currentMeleeWeapon;
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(attackPosition.position, attackRadius);
    }
}
