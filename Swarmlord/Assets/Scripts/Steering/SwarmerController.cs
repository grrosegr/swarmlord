using UnityEngine;
using System.Collections;

public class SwarmerController : MonoBehaviour {
	public float maxAcceleration;
	public float maxRotation;
	public float maxSpeed;
	public float maxRotationSpeed;

	public Steering test;
	public Arrive arriveContributer;
	public Align alignContributer;
	public Separate sepContributer;

	public Vector3 velocity;
	public float rotationVelo;
	public GameObject myTarget;

	public BlendedSteering bsTest;

	// Use this for initialization
	void Start () {
		velocity = new Vector3 (0, 0, 0);
		arriveContributer = new Arrive (myTarget, this);
		alignContributer = new Align (myTarget, this);
		bsTest = new BlendedSteering (this);
		sepContributer = new Separate (this);
	}
	
	// Update is called once per frame
	void Update () {
		SteerUpdate ();
	}

	//Page 60
	void SteerUpdate () {
		//Steering nextArrive = arriveContributer.GetSteering ();
		//Steering nextAlign = alignContributer.GetSteering ();

		//Blend the steerings
		bsTest.AddBehavior (arriveContributer, 0.5f);
		bsTest.AddBehavior (alignContributer, 0.5f);
		bsTest.AddBehavior (sepContributer, 2.0f);

		Steering blendedBehavior = bsTest.GetSteering ();

		//Steering nextArrive = arriveContributer.GetSteering ();
		//Steering nextAlign = alignContributer.GetSteering ();
		//Steering nextSep = sepContributer.GetSteering ();


		transform.position = transform.position + (velocity * Time.deltaTime);
		transform.Rotate (0, 0, rotationVelo * Time.deltaTime);

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
