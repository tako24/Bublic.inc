using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float Speed = 5f;
	public float DashCD = 1f;
	public float DashSpeed = 1;
	public float DashTime = 1.0f;

	private Vector2 movementVector;
    private Direction direction;
    private Rigidbody2D rb;
	private float _currentCD = 0;
	private Animator animator;
	private GameObject _ap;
	private bool _isDashing;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		rb.freezeRotation = true;
		rb.gravityScale = 0;
		_ap = GetComponentInChildren<MeleeWeapon>().gameObject;
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

		if (movementVector.Equals(Vector2.zero))
		{
			animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0);
			animator.speed = 0f;
		}

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

    void SetPlayerDirection()
    {
		movementVector.Normalize();
		if (movementVector == Vector2.up)
		{
			direction = Direction.Up;
			_ap.transform.rotation = Quaternion.Euler(0, 0, 45);
			return;
		}
        if (movementVector == Vector2.down)
        {
			direction = Direction.Down;
			_ap.transform.rotation = Quaternion.Euler(0, 0, -135);
			return;
		}
        if (movementVector == Vector2.left)
        {
			direction = Direction.Left;
			_ap.transform.rotation = Quaternion.Euler(0, 0, 135);
			return;
		}
		if (movementVector == Vector2.right)
		{
			direction = Direction.Right;
			_ap.transform.rotation = Quaternion.Euler(0, 0, -45);
			return;
		}

        if (movementVector.x > 0)
        {
			if (movementVector.y > 0) 
			{
				direction = Direction.UpRight;
				_ap.transform.rotation = Quaternion.Euler(0, 0, 0);
				return;
			}
			if (movementVector.y < 0)
			{
				direction = Direction.DownRight;
				_ap.transform.rotation = Quaternion.Euler(0, 0, -90);
				return;
			}
			return;
		}
        if (movementVector.x < 0)
        {
			if (movementVector.y > 0)
			{
				direction = Direction.UpLeft;
				_ap.transform.rotation = Quaternion.Euler(0, 0, 90);
				return;
			}
			if (movementVector.y < 0)
			{
				direction = Direction.DownLeft;
				_ap.transform.rotation = Quaternion.Euler(0, 0, -180);
				return;
			}
		}

        //print(direction);
    }
}
