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
	private GameObject _weapon;
	private bool _isDashing;
	public int _score;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		rb.freezeRotation = true;
		rb.gravityScale = 0;
		_weapon = GetComponentInChildren<MeleeWeapon>().gameObject;
	}

	void FixedUpdate()
	{
		rb.velocity = Vector2.zero;
		if (_isDashing)
			rb.MovePosition(rb.position + movementVector * DashSpeed * Time.fixedDeltaTime);
		else
			rb.MovePosition(rb.position + movementVector * Speed * Time.fixedDeltaTime);

		Animate();
	}

	void Update()
	{
		if (_isDashing) return;

		movementVector.x = Input.GetAxis("Horizontal");
		movementVector.y = Input.GetAxis("Vertical");
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
		float startTime = Time.time;
		while (Time.time < startTime + DashTime)
		{
			//transform.Translate(_direction * DashSpeed * Time.deltaTime);
			//rb.MovePosition(rb.position + movementVector * DashSpeed * Time.fixedDeltaTime);
			yield return null;
		}
		_isDashing = false;
	}

	void LookAtCursor()
	{
		Vector3 lookPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
		lookPos -= transform.position;
		lookPos.z = 0;
		float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
		_weapon.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
		var desiredPosition = transform.position + lookPos.normalized * 0.5f;
		_weapon.transform.position = Vector3.MoveTowards(_weapon.transform.position, desiredPosition, Time.deltaTime * 80f);
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
