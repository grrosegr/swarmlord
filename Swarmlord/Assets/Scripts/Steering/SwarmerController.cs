using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SwarmerController : MonoBehaviour {
	public float maxAcceleration;
	public float maxRotation;
	public float maxSpeed;
	public float maxRotationSpeed;

	public float attackBeatlesWeight;

	public float weight_Arrive;
	public float weight_Align;
	public float weight_Sep;
	public float weight_Avoid;
	public float weight_Wander;
	public float weight_VelocityMatch;

	public Steering test;
	public Arrive arriveContributer;
	public Align alignContributer;
	public Separate sepContributer;
	public AvoidObstacle avoidContributer;
	public Arrive nextBeatlesTarget;
	public Wander wander;
	public VelocityMatch velocityMatchContributer;
	public Arrive lastKnownContributer;

	public GameObject myTarget;

	public Vector2 velocity;
	public Vector3 lastKnownLocation;
	
	float rotationVelo;

	public BlendedSteering bsTest;
	
	public float MaxAngleVisible = 45.0f; // degrees
	
	private GameObject[] players;
	
	public GameObject ScreamPrefab;
	public AudioClip ScreamSound;

	// Use this for initialization
	void Start () {
		velocity = new Vector3 (0, 0, 0);
		arriveContributer = new Arrive (myTarget.transform.position, this);
		alignContributer = new Align (myTarget, this);
		bsTest = new BlendedSteering (this);
		sepContributer = new Separate (this);
		avoidContributer = new AvoidObstacle (this);
		nextBeatlesTarget = new Arrive (Vector3.zero, this);
		wander = new Wander(this);
		velocityMatchContributer = new VelocityMatch(this);

		attackBeatlesWeight = 0.0f;

		players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		SteerUpdate ();
		
		SpriteRenderer r = (SpriteRenderer)renderer;
		
		if (Time.time > screamResetTime) {
			screamed = false;
			r.color = Color.white;
		}
		
		foreach (GameObject go in players) {
			if (go == null) continue;
			if (CanSee(go) && go.GetComponent<CharacterController2D>().Alive) {
				Scream(go.transform.position);
				AddNewArriveLocation (go.transform.position);
				break;
			}
		}
	}

	public Vector2 GetVelo () {
		return velocity;
	}

	public void AddNewArriveLocation (Vector3 loc) {
		nextBeatlesTarget = new Arrive (loc, this);
		attackBeatlesWeight = 20.0f;
	}

	//Page 60
	void SteerUpdate () {
		//Steering nextArrive = arriveContributer.GetSteering ();
		//Steering nextAlign = alignContributer.GetSteering ();
		//Steering nextArrive = arriveContributer.GetSteering ();
		//Steering nextAlign = alignContributer.GetSteering ();
		//Steering nextSep = sepContributer.GetSteering ();
		bsTest.ResetList ();

		//Blend the steerings
		bsTest.AddBehavior (arriveContributer, weight_Arrive);
		bsTest.AddBehavior (alignContributer, weight_Align);
		bsTest.AddBehavior (sepContributer, weight_Sep);
		bsTest.AddBehavior (avoidContributer, weight_Avoid);
		bsTest.AddBehavior (wander, weight_Wander);
		bsTest.AddBehavior (velocityMatchContributer, weight_VelocityMatch);
		//bsTest.AddBehavior (nextBeatlesTarget, attackBeatlesWeight);

		//If no Beatle has been seen, go to the lastKnownLocation
		if (nextBeatlesTarget.target == Vector3.zero) {
			//print (lastKnownLocation);
			lastKnownContributer = new Arrive(lastKnownLocation, this);
			bsTest.AddBehavior (lastKnownContributer, attackBeatlesWeight);
		} else {
			//A Beatle has been seen. Go to Beatle location.
			//print ("meep");
			bsTest.AddBehavior (nextBeatlesTarget, attackBeatlesWeight);
		}

		Steering blendedBehavior = bsTest.GetSteering ();

		rigidbody2D.MovePosition(rigidbody2D.position + (Vector2)(velocity * Time.deltaTime));
		rigidbody2D.MoveRotation(rigidbody2D.rotation + rotationVelo * Time.deltaTime);

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
		Vector2 m_fwd = transform.up;
		Vector2 to_target = t_pos - m_pos;
		float angle = Vector2.Angle(to_target, m_fwd);
		
		if (!(Mathf.Abs(angle) < MaxAngleVisible))
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
		SpriteRenderer r = (SpriteRenderer)renderer;
		r.color = Color.red;
		
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

		if (scream.LastSeenTime > lastSeenTime) {
			lastKnownLocation = scream.LastSeenPosition;
			lastSeenTime = scream.LastSeenTime;
		}
	}

}
