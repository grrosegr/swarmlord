using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {

	public float Speed = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = transform.position;
		pos.x += Input.GetAxis("Horizontal") * Speed;
		pos.y += Input.GetAxis("Vertical") * Speed;
		
		transform.position = pos;
	
	}
}
