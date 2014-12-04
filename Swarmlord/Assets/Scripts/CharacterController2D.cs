using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterController2D : MonoBehaviour {

	public float Speed = 0.5f;
	const float FastSpeed = 10.0f;
	private float startSpeed;
	
	private bool IsControlled;
	
	public float MaxHealth = 100f;
	public float SwarmDamagePerSecond = 100f;
	
	private bool _alive;
	public bool Alive {
		get {
			return _alive;
		}
		
		private set {
			_alive = value;
			if (!_alive && IsControlled) {
				CharacterManager.Instance.SwitchToAlivePlayer();
			}
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
	
	private Animator anim;

	// Use this for initialization
	void Start () {
		Health = MaxHealth;
		anim = GetComponent<Animator>();
		startSpeed = Speed;
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.G)) {
			if (Speed == FastSpeed)
				Speed = startSpeed;
			else
				Speed = FastSpeed;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!_alive && !IsControlled)
			Destroy(gameObject);
	
		if (!(IsControlled && Alive))
			return;
	
		Vector2 velocity = new Vector2();
		velocity.x = Input.GetAxis("Horizontal");
		velocity.y = Input.GetAxis("Vertical");
		if (velocity != Vector2.zero)
			velocity.Normalize();
		velocity *= Speed;
		
		anim.SetBool("Walking", velocity != Vector2.zero);
		Vector2 localScale = transform.localScale;
		if (velocity.x > 0)
			localScale.x = Mathf.Abs(localScale.x);
		else if (velocity.x < 0)
			localScale.x = -Mathf.Abs (localScale.x);
		transform.localScale = localScale;
		
		rigidbody2D.MovePosition(rigidbody2D.position + velocity * Time.fixedDeltaTime);
		
		
		int swarmersCurrentlyColliding = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("Swarmer")).Length;
		if (swarmersCurrentlyColliding > 0)
			Health -= swarmersCurrentlyColliding * SwarmDamagePerSecond * Time.fixedDeltaTime;
	}
	
	void SetIsControlled(bool isControlled) {
		this.IsControlled = isControlled;
		// TODO: remove DontRequireReceiver
		BroadcastMessage("SelectedChanged", isControlled, SendMessageOptions.DontRequireReceiver);
		if (!isControlled)
			anim.SetBool("Walking", false);
	}
	
	private int swarmersCurrentlyColliding;
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.tag == "swarmer")
			swarmersCurrentlyColliding += 1;
	}
	
	void OnCollisionExit2D(Collision2D collision) {
		if (collision.collider.tag == "swarmer")
			swarmersCurrentlyColliding -= 1;
	}
	
	public void ApplyDamage(float damage) {
		Health -= damage;
	}
}
