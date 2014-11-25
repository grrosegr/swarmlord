﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterController2D : MonoBehaviour {

	public float Speed = 0.5f;
	private bool IsControlled;
	
	public float MaxHealth = 100f;
	
	private bool _alive;
	public bool Alive {
		get {
			return _alive;
		}
		
		private set {
			_alive = value;
			if (!_alive && IsControlled)
				CharacterManager.Instance.SwitchToAlivePlayer();
		}
	}
	
	private float _health;
	private float Health {
		get { return _health; }
		set {
			_health = Mathf.Max(0, value);
			UpdateColor();
			Alive = _health > 0;
		}
	}
	
	private void UpdateColor() {
		SpriteRenderer spriteRenderer = (SpriteRenderer)this.renderer;
		Color color = spriteRenderer.color;
		if (Health > 0) {
			color.r = 1f;
			float healthFraction = Health/MaxHealth;
			color.g = healthFraction;
			color.b = healthFraction;
		} else
			color = Color.black;
		spriteRenderer.color = color;
	}

	// Use this for initialization
	void Start () {
		Health = MaxHealth;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!(IsControlled && Alive))
			return;
	
		Vector2 velocity = new Vector2();
		velocity.x = Input.GetAxis("Horizontal") * Speed;
		velocity.y = Input.GetAxis("Vertical") * Speed;
		
		rigidbody2D.MovePosition(rigidbody2D.position + velocity * Time.deltaTime);
	}
	
	void SetIsControlled(bool isControlled) {
		this.IsControlled = isControlled;
	}
	
	public void ApplyDamage(float damage) {
		Health -= damage;
	}
}
