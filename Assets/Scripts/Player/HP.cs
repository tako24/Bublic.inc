using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public Text healthUI;
    private int CurrentHP;
    private int MaxHP;
    private void Start()
    {
        CurrentHP = 100;
        MaxHP = 150;
        healthUI.text = CurrentHP.ToString();
    }
    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
            Die();
        healthUI.text = CurrentHP.ToString();
    }
    public void Heal(int heal)
    {
        CurrentHP += heal;
        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;
        healthUI.text = CurrentHP.ToString();
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
