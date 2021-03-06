﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SwarmerController : MonoBehaviour {
	public bool DebugMode;
	public float maxAcceleration;
	public float maxRotationAcceleration;
	public float maxSpeed;
	public float maxRotationSpeed;

	public float weight_AttackBeatles;
	public float weight_FollowObject;
	public float weight_Align;
	public float weight_Sep;
	public float weight_Avoid;
	public float weight_Wander;
	public float weight_VelocityMatch;
	public float weight_FollowPath;
	public float weight_Home;

	public Steering test;
	public FollowObject followObjectContributer;
	public Align alignContributer;
	public Separate sepContributer;
	public AvoidObstacle avoidContributer;
	public Wander wander;
	public VelocityMatch velocityMatchContributer;
	public Arrive lastKnownContributer;
	public FollowPath followPathContributer;
	public StayAtHome homeContributer;

	public GameObject myTarget;

	public Vector2 velocity;
	public Vector3 lastKnownLocation;
	
	public GameObject Path;
	
	float rotationVelo;

	public BlendedSteering bsTest;
	
	public float MaxAngleVisible = 45.0f; // degrees
	public float MaxDistanceVisible = 10.0f;
	
	private GameObject[] players;
	
	public GameObject ScreamPrefab;
	public AudioClip ScreamSound;
	
	private GameObject currentTarget;
	private Animator anim;
	
	public Vector2 GetForward() {
		if (transform.localScale.x >= 0)
			return transform.right;
		else
			return -transform.right;
	}

	// Use this for initialization
	void Start () {
		velocity = new Vector3 (0, 0, 0);
		followObjectContributer = new FollowObject(myTarget, this);
		alignContributer = new Align (myTarget, this);
		bsTest = new BlendedSteering (this);
		sepContributer = new Separate (this);
		avoidContributer = new AvoidObstacle (this);
		wander = new Wander(this);
		homeContributer = new StayAtHome(transform.position, this);
		velocityMatchContributer = new VelocityMatch(this);
		followPathContributer = new FollowPath(Path, this);

		players = GameObject.FindGameObjectsWithTag("Player");
		
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		SteerUpdate ();
		
		SpriteRenderer r = (SpriteRenderer)renderer;
		
		if (Time.time > screamResetTime) {
			screamed = false;
			r.color = Color.white;
		}
		
		Vector2 fwd = GetForward();
		Debug.DrawRay(transform.position, fwd.GetRotatedByDegrees(-MaxAngleVisible) * MaxDistanceVisible);
		Debug.DrawRay(transform.position, fwd.GetRotatedByDegrees(MaxAngleVisible) * MaxDistanceVisible);
		
		if (currentTarget && !CanSee(currentTarget))
			currentTarget = null;
		
		if (!currentTarget) {
			foreach (GameObject go in players) {
				if (go == null) continue;
				if (CanSee(go) && go.GetComponent<CharacterController2D>().Alive) {
					currentTarget = go;
					lastKnownLocation = go.transform.position;
					lastSeenTime = Time.time;
					break;
				}
			}
		}	
		
		if (currentTarget)
			Scream(currentTarget.transform.position);
	}

	public Vector2 GetVelocity () {
		return velocity;
	}

	//Page 60
	void SteerUpdate () {
		bsTest.ResetList ();

		//Blend the steerings
		bsTest.AddBehavior (followObjectContributer, weight_FollowObject);
		bsTest.AddBehavior (alignContributer, weight_Align);
		bsTest.AddBehavior (sepContributer, weight_Sep);
		bsTest.AddBehavior (avoidContributer, weight_Avoid);
		bsTest.AddBehavior (wander, weight_Wander);
		bsTest.AddBehavior (velocityMatchContributer, weight_VelocityMatch);
		
		SpriteRenderer r = (SpriteRenderer)renderer;
		
		//If no Beatle has been seen, go to the lastKnownLocation
		if (lastKnownLocation != Vector3.zero && (Time.time - lastSeenTime) < 5f) {
			if (DebugMode)
				Debug.Log ("last known");
			lastKnownContributer = new Arrive(lastKnownLocation, this);
			bsTest.AddBehavior (lastKnownContributer, weight_AttackBeatles);
			anim.SetInteger("Mode", 1);
		} else {
			if (DebugMode)
				Debug.Log (lastKnownLocation == Vector3.zero && Path == null && myTarget == null);
			if (lastKnownLocation == Vector3.zero && Path == null && myTarget == null)
				bsTest.AddBehavior(homeContributer, weight_Home);
			bsTest.AddBehavior (followPathContributer, weight_FollowPath);
			
			anim.SetInteger("Mode", 0);
		}

		Steering blendedBehavior = bsTest.GetSteering ();

		rigidbody2D.MovePosition(rigidbody2D.position + (Vector2)(velocity * Time.deltaTime));
		Vector3 localScale = transform.localScale;
		if (velocity.x > 0)
			localScale.x = Mathf.Abs(localScale.x);
		else if (velocity.x < 0)
			localScale.x = -Mathf.Abs (localScale.x);
		if (localScale != transform.localScale)
			transform.localScale = localScale;
//		if (DebugMode)
//			Debug.Log (velocity);
			
		anim.SetBool("Walking", !Mathf.Approximately(velocity.magnitude, 0));
			
		//rigidbody2D.MoveRotation(rigidbody2D.rotation + rotationVelo * Time.deltaTime);

		velocity += blendedBehavior.linear * Time.deltaTime;
		if (blendedBehavior.stop)
			rotationVelo = 0;
		else
			rotationVelo += blendedBehavior.angular * Time.deltaTime;

		if (velocity.magnitude > maxSpeed) {
			velocity = velocity.normalized * maxSpeed;
		}
		
		rotationVelo = Mathf.Clamp(rotationVelo, -maxRotationSpeed, maxRotationSpeed);
	}

	bool CanSee(GameObject other) {
		Vector2 t_pos = other.transform.position;
		Vector2 m_pos = transform.position;
		Vector2 m_fwd = GetForward();
		Vector2 to_target = t_pos - m_pos;
		float angle = Vector2.Angle(to_target, m_fwd);
		
		if (!(Mathf.Abs(angle) < MaxAngleVisible))
			return false;
		
		if (!(Vector2.Distance(transform.position, other.transform.position) < MaxDistanceVisible))
			return false;
		
		RaycastHit2D hit = Physics2D.Raycast(m_pos, to_target, to_target.magnitude, LayerMask.GetMask("Obstacle"));
		
		return hit.collider == null;
	}
	
	private bool screamed = false;
	private float screamResetTime;
	
	public float ScreamResetAfter = 5.0f;
	
	void Scream(Vector2 lastSeenPos) {
		if (screamed)
			return;
		
		//new seek source here
		
		audio.clip = ScreamSound;
		audio.Stop();
		audio.Play ();
		screamed = true;
		
		GameObject scream = (GameObject)Instantiate(ScreamPrefab, transform.position, Quaternion.identity);
		Scream controller = scream.GetComponent<Scream>();
		controller.LastSeenTime = Time.time;
		controller.LastSeenPosition = lastSeenPos;
		
		screamResetTime = Time.time + ScreamResetAfter;
	}
	
	private float lastSeenTime;
	void OnScream(Scream scream) {
		// TODO: maybe scream again?
//		Scream();

		if (!currentTarget && scream.LastSeenTime > lastSeenTime) {
			lastKnownLocation = scream.LastSeenPosition;
			lastSeenTime = scream.LastSeenTime;
		}
	}

}
