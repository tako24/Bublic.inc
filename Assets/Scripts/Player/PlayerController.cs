using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 5f;
	public Camera cam;
	private Vector2 movement;
	private Vector2 mousePosition;
	private Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.freezeRotation = true;
		rb.gravityScale = 0;

	}

	void FixedUpdate()
	{
		rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
		Vector2 lookDirection = mousePosition - rb.position;
		float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
		rb.rotation = angle;
	}

	//void LookAtCursor()
	//{
	//	Vector3 lookPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
	//	lookPos = lookPos - transform.position;
	//	float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
	//	transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	//}

	void Update()
	{
		movement.x = Input.GetAxis("Horizontal");
		movement.y = Input.GetAxis("Vertical");

		mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
	}
}