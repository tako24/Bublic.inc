using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    private int CurrentHP;
    private int MaxHP;


    public Color textColor = Color.white;
    public float textHeight = 0.8f;
    public Color shadowColor = new Color(0, 0, 0, 0.5f);
    public Vector2 shadowOffset = new Vector2(1, 1);

    GUIStyle style = new GUIStyle();


    private void Start()
    {
        style.richText = true;
        style.normal.textColor = textColor;
        style.alignment = TextAnchor.MiddleCenter;
        CurrentHP = 100;
        MaxHP = 150;
    }
    void OnGUI()
    {
        GUI.depth = 9999;

        style.normal.textColor = textColor;


        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + textHeight, transform.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition.y = Screen.height - screenPosition.y;

        GUI.Label(new Rect(screenPosition.x, screenPosition.y, 0, 0), CurrentHP.ToString(), style);
    }
<<<<<<< HEAD
    public Text healthUI;
    private int CurrentHP;
    private int MaxHP;
    public  void TakeDamage(int damage)
=======
    public void TakeDamage(int damage)
>>>>>>> parent of a51ef0a (HPBAR)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0)
            Die();
        healthUI.text = CurrentHP.ToString();
    }
    public void Heal(int heal)
    {
<<<<<<< HEAD
        _currentHP += heal;
        if (_currentHP > _maxHP)
            _currentHP = _maxHP;
        CurrentHP += heal;
        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;
        healthUI.text = CurrentHP.ToString();
=======
        CurrentHP += heal;
        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;
>>>>>>> parent of a51ef0a (HPBAR)
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
