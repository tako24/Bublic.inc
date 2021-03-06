using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float Speed = 5f;
	public float DashCD = 1f;
	public float DashSpeed = 1;
	public float DashTime = 1.0f;
	public Transform _holdP;
	private Vector2 movementVector;
    private Direction direction;
    private Rigidbody2D rb;
	private float _currentCD = 0;
	private Animator animator;
	public MeleeWeapon Weapon;
	private bool _isDashing;
	public int Score;
	public AudioClip coinsSound;
	public Collider2D Hitbox;
	public SpriteRenderer PlayerSprite;

	void Start()
	{
		PlayerSprite = GetComponent<SpriteRenderer>();
		Hitbox = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		rb.freezeRotation = true;
		rb.gravityScale = 0;
	}

	void FixedUpdate()
	{
		rb.velocity = Vector2.zero;
		if (_isDashing)
		{
			rb.MovePosition(rb.position + movementVector * DashSpeed * Time.fixedDeltaTime);
		}
		else
		{
			rb.MovePosition(rb.position + movementVector * Speed * Time.fixedDeltaTime);
		}

		Animate();
	}

	void Update()
	{
		if (Time.timeScale == 0f || _isDashing) return;

		movementVector.x = Input.GetAxis("Horizontal");
		movementVector.y = Input.GetAxis("Vertical");

		if (Weapon)
			LookAtCursor();

		SetPlayerDirection();

		if (Input.GetKeyDown(KeyCode.LeftShift) && !_isDashing && _currentCD <= 0)
		{
			StartCoroutine(DashCoroutine());
			_isDashing = true;
			_currentCD = DashCD;
		}
		else
			_currentCD -= Time.deltaTime;
	}

	private IEnumerator DashCoroutine()
	{
		PlayerSprite.color = new Color(1, 1, 1, 0.5f);
		Hitbox.enabled = false;
		float startTime = Time.time;
		while (Time.time < startTime + DashTime)
		{
			//transform.Translate(_direction * DashSpeed * Time.deltaTime);
			//rb.MovePosition(rb.position + movementVector * DashSpeed * Time.fixedDeltaTime);
			yield return null;
		}
		PlayerSprite.color = new Color(1, 1, 1, 1f);
		Hitbox.enabled = true;
		_isDashing = false;
	}

	void LookAtCursor()
	{
		Vector3 lookPos = Camera.main.ScreenToWorldPoint(
			new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
		lookPos -= transform.position;
		lookPos.z = 0;

		float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
		Weapon.transform.rotation = Quaternion.AngleAxis(angle - Weapon.AngleOffset, Vector3.forward);
		var desiredPosition = transform.position + lookPos.normalized * Weapon.DistanceOffset;
		Weapon.transform.position = Vector3.MoveTowards(Weapon.transform.position, desiredPosition, Time.deltaTime * 80f);
	}

	void Animate()
	{
		animator.speed = 1f;

		switch (direction)
		{
			case Direction.Up:
				SetAnimation("PlayerLooksUp");
				break;
			case Direction.Down:
				SetAnimation("PlayerLooksDown");
				break;
			case Direction.Right:
				SetAnimation("PlayerLooksRight");
				break;
			case Direction.Left:
				SetAnimation("PlayerLooksLeft");
				break;
			case Direction.UpRight:
				SetAnimation("PlayerLooksUp");
				break;
			case Direction.UpLeft:
				SetAnimation("PlayerLooksUp");
				break;
			case Direction.DownRight:
				SetAnimation("PlayerLooksDown");
				break;
			case Direction.DownLeft:
				SetAnimation("PlayerLooksDown");
				break;
			default:
				break;
		}

		////if (movementVector.Equals(Vector2.zero))
		//{
		//	animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0);
		//	animator.speed = 0f;
		//}

		//print(movementVector.ToString() + " " + direction);
	}

	void SetAnimation(string currentState)
	{
		animator.SetBool(currentState, true);

		foreach (var state in new[] { "PlayerLooksUp", "PlayerLooksDown", "PlayerLooksRight", "PlayerLooksLeft" })
		{
			if (!state.Equals(currentState))
				animator.SetBool(state, false);
		}
	}

	public delegate void OnCoinTake();
	public event OnCoinTake onCoinTake;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Coin"))
		{
			TakeCoin();
			Destroy(collision.gameObject);
		}
	}

	private void TakeCoin()
	{
		onCoinTake?.Invoke();
		gameObject.GetComponent<AudioSource>().PlayOneShot(coinsSound);
	}

	void SetPlayerDirection()
    {
		movementVector.Normalize();
		if (movementVector == Vector2.up)
		{
			direction = Direction.Up;
			return;
		}
        if (movementVector == Vector2.down)
        {
			direction = Direction.Down;
			return;
		}
        if (movementVector == Vector2.left)
        {
			direction = Direction.Left;
			return;
		}
		if (movementVector == Vector2.right)
		{
			direction = Direction.Right;
			return;
		}

        if (movementVector.x > 0)
        {
			if (movementVector.y > 0) 
			{
				direction = Direction.UpRight;
				return;
			}
			if (movementVector.y < 0)
			{
				direction = Direction.DownRight;
				return;
			}
			return;
		}
        if (movementVector.x < 0)
        {
			if (movementVector.y > 0)
			{
				direction = Direction.UpLeft;
				return;
			}
			if (movementVector.y < 0)
			{
				direction = Direction.DownLeft;
				return;
			}
		}

        //print(direction);
    }
}
