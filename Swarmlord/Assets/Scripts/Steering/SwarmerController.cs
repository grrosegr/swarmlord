using UnityEngine;
using System.Collections;

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

	public Steering test;
	public Arrive arriveContributer;
	public Align alignContributer;
	public Separate sepContributer;
	public AvoidObstacle avoidContributer;
	public Arrive nextBeatlesTarget;

	public GameObject myTarget;

	public Vector3 velocity;
	float rotationVelo;

	public BlendedSteering bsTest;

	// Use this for initialization
	void Start () {
		velocity = new Vector3 (0, 0, 0);
		arriveContributer = new Arrive (myTarget.transform.position, this);
		alignContributer = new Align (myTarget, this);
		bsTest = new BlendedSteering (this);
		sepContributer = new Separate (this);
		avoidContributer = new AvoidObstacle (this);
		nextBeatlesTarget = new Arrive (Vector3.zero, this);

		attackBeatlesWeight = 0.0f;

	}
	
	// Update is called once per frame
	void Update () {
		SteerUpdate ();
	}

	public Vector3 GetVelo () {
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
		bsTest.AddBehavior (nextBeatlesTarget, attackBeatlesWeight);

		Steering blendedBehavior = bsTest.GetSteering ();

		//transform.position = transform.position + (velocity * Time.deltaTime);
		rigidbody2D.MovePosition(transform.position + (velocity * Time.deltaTime));
		//transform.Rotate (0, 0, rotationVelo * Time.deltaTime);

		velocity = velocity + (blendedBehavior.linear * Time.deltaTime);
		if (blendedBehavior.stop)
			rotationVelo = 0;
		else
			rotationVelo = rotationVelo + (blendedBehavior.angular * Time.deltaTime);

		if (velocity.magnitude > maxSpeed) {
			velocity = velocity.normalized * maxSpeed;
		}

		if (rotationVelo > maxRotationSpeed) {
			rotationVelo = maxRotationSpeed;
		}
		else if (rotationVelo < -maxRotationSpeed) {
			rotationVelo = -maxRotationSpeed;
		}
	}

	/*
	void BlendSteering () {
		Steering finalSteering = new Steering ();
	}*/



}
