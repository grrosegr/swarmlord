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

	public Vector3 velocity;
	public float rotationVelo;
	public GameObject myTarget;
	
	// Use this for initialization
	void Start () {
		velocity = new Vector3 (0, 0, 0);
		arriveContributer = new Arrive (myTarget, this);
		alignContributer = new Align (myTarget, this);
	}
	
	// Update is called once per frame
	void Update () {
		SteerUpdate ();
	}

	//Page 60
	void SteerUpdate () {
		Steering nextArrive = arriveContributer.GetSteering ();
		Steering nextAlign = alignContributer.GetSteering ();

		transform.position = transform.position + (velocity * Time.deltaTime);
		transform.Rotate (0, 0, rotationVelo * Time.deltaTime);

		velocity = velocity + (nextArrive.linear * Time.deltaTime);
		if (nextAlign.stop)
			rotationVelo = 0;
		else
			rotationVelo = rotationVelo + (nextAlign.angular * Time.deltaTime);

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

	void BlendSteering () {
		Steering finalSteering = new Steering ();
	}



}
