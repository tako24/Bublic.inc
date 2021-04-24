using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 5f;
	private Vector2 movement;
	private Rigidbody2D rb;
	public float _dashSpeed = 200000;
	private float _currentCD = 0;
	private float _dashCD = 1f;

	public float DashCD = 1f;
	public float DashSpeed = 1;
	public float DashTime = 1.0f;
	private Vector2 movementVector;

	private Vector2 _direction;
	private Direction direction;
	private Animator animator;
	private bool _isDashing;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.freezeRotation = true;
		rb.gravityScale = 0;


	}

	void FixedUpdate()
	{
		rb.velocity = Vector2.zero;

		rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
		float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
		rb.rotation = angle;
	}

	void LookAtCursor()
	{
		Vector3 lookPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
		lookPos = lookPos - transform.position;
		float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	void Update()
	{
		if (_isDashing) return;

		movementVector.x = Input.GetAxis("Horizontal");
		movementVector.y = Input.GetAxis("Vertical");
		_direction = movementVector.normalized;
		LookAtCursor();

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
			transform.Translate(_direction * DashSpeed * Time.deltaTime);
			yield return null;
		}
		_isDashing = false;
	}


	public void Dash()
	{
		Vector2 mouseDirection = (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2)).normalized;
		rb.AddForce(mouseDirection * _dashSpeed * Time.fixedDeltaTime);
	}
}