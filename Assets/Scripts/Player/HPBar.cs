using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;
    
    private int _currentHP;
    private int _maxHP;

    public Color textColor = Color.white;
    public float textHeight = 0.8f;
    public Color shadowColor = new Color(0, 0, 0, 0.5f);
    public Vector2 shadowOffset = new Vector2(1, 1);

    GUIStyle style = new GUIStyle();

    void OnGUI()
    {
        GUI.depth = 9999;

        style.normal.textColor = textColor;

        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + textHeight, transform.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition.y = Screen.height - screenPosition.y;

        GUI.Label(new Rect(screenPosition.x, screenPosition.y, 0, 0), _currentHP.ToString(), style);
    }
    private void Start()
    {
        _currentHP = 100;
        _maxHP = 100;
        _slider.maxValue = _maxHP;
        _slider.value = _currentHP;
        _fill.color = _gradient.Evaluate(1f);
    }
    public  void TakeDamage(int damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0)
            Die();
        _slider.value = _currentHP;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
    public  void Heal(int heal)
    {
        _currentHP += heal;
        if (_currentHP > _maxHP)
            _currentHP = _maxHP;
        _slider.value = _currentHP;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
    public  void Die()
    {
        // как он умирает?
        Destroy(gameObject);
    }
}
