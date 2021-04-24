using System.Collections;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public int Ticks;
    public int TickDamage;
    public int TimeBetweenTicksInSeconds;

    private HPBar hpBar;
    private bool poisonInflicted;

    void Start()
    {
        hpBar = FindObjectOfType<HPBar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!poisonInflicted)
            StartCoroutine("InflictPoison");
    }

    IEnumerator InflictPoison()
    {
        print("Poison Inflicted");
        poisonInflicted = true;
        for (int i = 0; i < Ticks - 1; i++)
        {
            print("Took Poison Damage");
            hpBar.TakeDamage(TickDamage);
            yield return new WaitForSeconds(TimeBetweenTicksInSeconds);
        }
        print("Took Poison Damage");
        hpBar.TakeDamage(TickDamage);
        print("Poison Weared Off");
        poisonInflicted = false;
    }
}
