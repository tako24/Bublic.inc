using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    public Transform AttackPosition;
    public int Damage { get; set; }
    public int Durability { get; set; }
    public float AttackCooldown { get; set; }
    public float StartTimeAttack { get; set; } = 0.5f;

    public float AttackRadius;

    void Update()
    {
        if (AttackCooldown <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DoDamage();
                AttackCooldown = StartTimeAttack;
            }
        }
        else 
        {
            AttackCooldown -= Time.deltaTime;
            //print("Идет перезарядка");
        }

    }

    public void PickUp()
    {
        GetComponent<ObjectNameView>().enabled = false;
    }

    public void DoDamage()
    {
        Collider2D[] enemysCollider = Physics2D.OverlapCircleAll(
            AttackPosition.position, AttackRadius, LayerMask.GetMask("Enemy"));

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

        AttackCooldown = StartTimeAttack;
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
