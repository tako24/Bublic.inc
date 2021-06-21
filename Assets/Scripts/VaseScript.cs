using UnityEngine;

public class VaseScript : MonoBehaviour
{
    public System.Collections.Generic.List<GameObject> Loot;
    public System.Collections.Generic.List<int> LootChances;

    private Animator animator;
    private CapsuleCollider2D collider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    public void Break()
    {
        var rand = Random.Range(0 + GameController.LuckBonus, 100 - GameController.LuckBonus * 2);

        for (int i = 0; i < LootChances.Count; i++)
            if (rand < LootChances[i])
            {
                Instantiate(Loot[i],
                            new Vector3(transform.position.x, transform.position.y - 0.25f, 4),
                            Quaternion.identity);
                break;
            }

        collider.enabled = false;
        animator.SetTrigger("Broke");
    }
}
