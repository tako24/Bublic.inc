using UnityEngine;

public class HP : MonoBehaviour
{
    public int _currentHP;
    public int _maxHP;
    public float InvFrames;
    private float _currentInvTime;

    public SpriteRenderer EnemySprite;
    public Color textColor = Color.white;
    public float textHeight = 0.8f;
    public Color shadowColor = new Color(0, 0, 0, 0.5f);
    public Vector2 shadowOffset = new Vector2(1, 1);
    public AudioClip damageSound;
    GUIStyle style = new GUIStyle();

    private void Start()
    {
        EnemySprite = GetComponent<SpriteRenderer>();
    }

    void OnGUI()
    {
        GUI.depth = 9999;

        style.normal.textColor = textColor;

        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + textHeight, transform.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition.y = Screen.height - screenPosition.y;

        GUI.Label(new Rect(screenPosition.x, screenPosition.y, 0, 0), _currentHP.ToString(), style);
    }

    private void FixedUpdate()
    {
        if (_currentInvTime > 0)
        {
            _currentInvTime -= Time.fixedDeltaTime;

            if (_currentInvTime <= 0f)
            {
                _currentInvTime = 0f;
                EnemySprite.color = Color.white;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (_currentInvTime == 0f)
        {
            _currentInvTime = InvFrames;
            if (CompareTag("Enemy"))
                EnemySprite.color = Color.red;
            _currentHP -= damage;
            gameObject.GetComponent<AudioSource>().PlayOneShot(damageSound);
            if (_currentHP <= 0)
                Die();
        }
    }

    public void Heal(int heal)
    {
        _currentHP += heal;
        if (_currentHP > _maxHP)
            _currentHP = _maxHP;
    }

    public void Die()
    {
        if (CompareTag("Enemy"))
        {
            GameController.CurrentRoom.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
        else if (CompareTag("Destroyable"))
        {
            GetComponent<VaseScript>().Break();
        }
    }
}
