using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ant : MonoBehaviour {

	public GameObject marker;

	private enum State {Wander, Follow, ReturnAndMark, Return}

	private Vector2 colonyCenter;
	private Vector2 direction;
	private float directionAngle; // radians
	private State state;
	
	public float Speed = 3.0f;
	public float MaxDistanceToColony = 12.0f;
	public float PlayerDetectionRadius = 2.0f;
	public float PheromoneDetectionRadius = 1.0f;
	
	private void SetTarget(Vector2 pos) {
		SetDirection(pos - (Vector2)transform.position);
	}
	
	private void SetDirection(float angle) {
		directionAngle = angle;
		direction = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle));
	}
	
	private void SetDirection(Vector2 direction) {
		this.direction = direction.normalized;
		directionAngle = Mathf.Atan2(direction.y, direction.x);

	}
	
	void ResetPath() {
		path.Clear();
		path.Add(colonyCenter);
	}

	// Use this for initialization
	void Start () {
		colonyCenter = transform.position;
		
		SetDirection(Random.value * 2 * Mathf.PI);
		state = State.Wander;
		path = new List<Vector2>();
		ResetPath();
	}
	
	private static float GetRandomBinomial() {
		// Returns a random number between −1 and 1, where values around zero are more likely.
		return Random.value - Random.value;
	}
	
	private Vector2 lastMark;
	
	private float returnStartTime;
	
	IList<Vector2> path;
	
	// Update is called once per frame
	void FixedUpdate () {		
//		float targetRot = directionAngle;
//		float myRot = transform.rotation.eulerAngles.z;
//		
//		float rotation = targetRot - myRot;
//		rotation += 360.0f;
//		rotation = Mathf.Repeat (rotation, 360.0f);
//		if (rotation > 180.0f) {
//			rotation -= 360.0f;
//		}
//		float rotationSign = Mathf.Sign(rotation);
//		float rotationSize = Mathf.Abs(rotation);
//		// TODO: rotations!
		
		rigidbody2D.MovePosition((Vector2)transform.position + direction * Speed * Time.fixedDeltaTime);
		rigidbody2D.MoveRotation((directionAngle - 90) * Mathf.Rad2Deg);

		SetDirection(directionAngle + GetRandomBinomial() * Mathf.PI * 0.05f);		
		
		if (state == State.Wander) {
			Bounds b = collider2D.bounds;
			
			Vector2 pos = transform.position;
			Vector2 last = path.Last();
			if (Vector2.Distance(pos, last) > 0.6f) {
				path.Add(pos);
			}
			
			if (Vector2.Distance(pos, colonyCenter) < 0.5f) {
				ResetPath();
			} else if (path.Count > 30) {
				state = State.ReturnAndMark;
			}
			
			Collider2D nearbyPlayer = Physics2D.OverlapCircle(transform.position, PlayerDetectionRadius, LayerMask.GetMask("Player"));
			if (nearbyPlayer && (Random.value < 0.8)) {
				SetTarget(nearbyPlayer.transform.position);
			} else {
				Collider2D[] markers = Physics2D.OverlapCircleAll(b.center, PheromoneDetectionRadius, LayerMask.GetMask("AntMarker"));
				if (Random.value < 0.2) {
					var good = markers.Where(marker => Vector2.Distance(colonyCenter, transform.position) < Vector2.Distance(colonyCenter, marker.transform.position)).ToList();
					if (good.Count > 0) {
						int index = (int)(Random.value * .99999 * good.Count);
						var theOne = good[index];
						Vector2 target = theOne.transform.position - transform.position;
						
						SetDirection(target);
					}
				}
			}
			
			if (Vector2.Distance(colonyCenter, transform.position) > MaxDistanceToColony
			    && Vector2.Dot((Vector2)transform.position - colonyCenter, this.direction) > 0) {
				SetDirection(direction * -1);
			}
		
			Collider2D hitPlayer = Physics2D.OverlapArea(b.min, b.max, LayerMask.GetMask("Player"));
			if (hitPlayer) {
				hitPlayer.SendMessage("ApplyDamage", 2.0f);
				
//				if (Random.value < 0.5f) {
					state = State.ReturnAndMark;
					SetDirection(colonyCenter - (Vector2)transform.position);
					returnStartTime = Time.time;
//				}
			}
		} else if (state == State.Return || state == State.ReturnAndMark) {
			if (path.Count > 0 && (Time.time - returnStartTime < 5.0f)) {
				Vector2 target = path.Last();
				
				if (Vector2.Distance(target, transform.position) > 0.5) {
					SetTarget(target);
				} else {
					path.RemoveAt(path.Count - 1);
				}
				
				if (state == State.ReturnAndMark) {
					int nearbyMarkers = Physics2D.OverlapCircleAll(transform.position, 0.2f, LayerMask.GetMask("AntMarker")).Length;
					
					if (Vector2.Distance(lastMark, transform.position) > 0.35f && nearbyMarkers < 8) {
						Instantiate(marker, transform.position, Quaternion.identity);
						lastMark = transform.position;
					}
				}
			} else {
				state = State.Wander;
				ResetPath();
			}
		}
	}
}
