using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed = 5f;
	private Vector2 movement;
	private Rigidbody2D rb;
	private bool isDashBtnDown;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.freezeRotation = true;
		rb.gravityScale = 0;

	}

	void FixedUpdate()
	{
		rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);		
		float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg - 90f;
		rb.rotation = angle;
		if (isDashBtnDown)
        {
			float dashAmount = 5f;  //расстояние
			print(movement.ToString());
			rb.MovePosition(rb.position + movement * dashAmount);
			isDashBtnDown = false;
        }
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
		movement.x = Input.GetAxis("Horizontal");
		movement.y = Input.GetAxis("Vertical");


		LookAtCursor();


		if (Input.GetKeyDown(KeyCode.E))
        {
			RaycastHit2D hit = Physics2D.Raycast(rb.position, movement, 10f,LayerMask.GetMask("Wall"));
            if (hit.collider !=null)
            {
				Debug.Log("Поймал" + hit.collider.gameObject);
				return;
			}
			isDashBtnDown = true;
        }
	}
}