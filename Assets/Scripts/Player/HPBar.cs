using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    //[SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;

    public int _currentHP;
    public int _maxHP;
    private float _currentInvTime;

    public Color TextColor = Color.white;
    public float TextHeight = 0.8f;
    public Color ShadowColor = new Color(0, 0, 0, 0.5f);
    public Vector2 ShadowOffset = new Vector2(1, 1);
    public AudioClip PlayerDamage;
    public float InvFrames;
    public SpriteRenderer PlayerSprite;

    GUIStyle style = new GUIStyle();

    void OnGUI()
    {
        GUI.depth = 9999;

        style.normal.textColor = TextColor;

        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + TextHeight, transform.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition.y = Screen.height - screenPosition.y;

        GUI.Label(new Rect(screenPosition.x, screenPosition.y, 0, 0), _currentHP.ToString(), style);
    }

    private void Start()
    {
        _slider.maxValue = _maxHP;
        _slider.value = _currentHP;
        //_fill.color = _gradient.Evaluate(1f);
        PlayerSprite.color = Color.white;
    }

    private void FixedUpdate()
    {
        if (_currentInvTime > 0)
        {
            _currentInvTime -= Time.fixedDeltaTime;

            if (_currentInvTime <= 0f)
            {
                _currentInvTime = 0f;
                PlayerSprite.color = Color.white;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (_currentInvTime == 0f)
        {
            _currentInvTime = InvFrames;
            PlayerSprite.color = Color.red;
            _currentHP -= damage;
            gameObject.GetComponent<AudioSource>().PlayOneShot(PlayerDamage);
            if (_currentHP <= 0)
                Die();
            _slider.value = _currentHP;
            //_fill.color = _gradient.Evaluate(_slider.normalizedValue);
        }
    }

    public void Heal(int heal)
    {
        _currentHP += heal;
        if (_currentHP > _maxHP)
            _currentHP = _maxHP;
        _slider.value = _currentHP;
        //_fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
