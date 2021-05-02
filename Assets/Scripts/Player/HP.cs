using UnityEngine;

public class HP : MonoBehaviour
{
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
        _maxHP = 150;
    }

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        if (_currentHP <= 0)
            Die();
    }

    public void Heal(int heal)
    {
        _currentHP += heal;
        if (_currentHP > _maxHP)
            _currentHP = _maxHP;
    }

    public void Die()
    {
        GameController.CurrentRoom.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }
}
