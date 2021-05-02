using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public int Damage;

    private HPBar player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<HPBar>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
            player.TakeDamage(Damage);
    }
}
