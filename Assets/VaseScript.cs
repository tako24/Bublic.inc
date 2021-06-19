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
        var lootIndex = Random.Range(0, Loot.Count);
        var lootChance = LootChances[lootIndex];
        var rand = Random.Range(0, 100);

        if (rand < lootChance)
            Instantiate(Loot[lootIndex], 
                        new Vector3(transform.position.x, transform.position.y - 0.25f, 4),
                        Quaternion.identity);

        collider.enabled = false;
        animator.SetTrigger("Broke");
    }
}
