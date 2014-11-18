using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {

	public float Speed = 0.5f;
	private bool IsControlled;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!IsControlled)
			return;
	
		Vector2 velocity = new Vector2();
		velocity.x = Input.GetAxis("Horizontal") * Speed;
		velocity.y = Input.GetAxis("Vertical") * Speed;
		
		rigidbody2D.MovePosition(rigidbody2D.position + velocity * Time.deltaTime);
	}
	
	void SetIsControlled(bool isControlled) {
		this.IsControlled = isControlled;
	}
}
