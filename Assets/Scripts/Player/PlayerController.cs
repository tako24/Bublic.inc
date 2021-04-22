using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float Speed = 5f;
	public float DashCD = 1f;
	public float DashSpeed = 200000;
	public Transform AttackPosition;

	private Vector2 movementVector;
	private Direction direction;
	private Rigidbody2D rb;
	private float _currentCD = 0;
	private Animator animator;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		rb.freezeRotation = true;
		rb.gravityScale = 0;
	}

	void FixedUpdate()
	{
		rb.velocity = Vector2.zero;
		rb.MovePosition(rb.position + movementVector * Speed * Time.fixedDeltaTime);

		Animate();
	}

	void Update()
	{
		movementVector.x = Input.GetAxis("Horizontal");
		movementVector.y = Input.GetAxis("Vertical");

		SetPlayerDirection();

		if (Input.GetKeyDown(KeyCode.LeftShift) && _currentCD <= 0)
		{
			Dash();
			_currentCD = DashCD;
		}
		else
			_currentCD -= Time.deltaTime;
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
		if (movementVector.x == 0)
		{
			if (movementVector.y > 0) direction = Direction.Up;
			else if (movementVector.y < 0) direction = Direction.Down;
		}
		else if (movementVector.y == 0)
		{
			if (movementVector.x < 0) direction = Direction.Left;
			else if (movementVector.x > 0) direction = Direction.Right;
		}
		else if (movementVector.x > 0)
        {
			if (movementVector.y > 0) direction = Direction.UpRight;
			else if (movementVector.y < 0) direction = Direction.DownRight;
        }
		else if (movementVector.x < 0)
		{
			if (movementVector.y > 0) direction = Direction.UpLeft;
			else if (movementVector.y < 0) direction = Direction.DownLeft;
		}

		print(direction);
	}

	public void Dash()
	{
		rb.AddForce(movementVector.normalized * DashSpeed * Time.fixedDeltaTime);
	}
}
