using UnityEngine;
using System.Collections;

public class Arrive : Behavior {
	public Vector3 target;
	SwarmerController character;

	public float maxAccel;
	public float maxSpeed;

	public float targetRadius = 0.5f;
	public float slowRadius = 10.0f;

	public float timeToTarget = 0.01f;

	public Arrive(Vector3 newTarget, SwarmerController newCharacter) {
		target = newTarget;
		character = newCharacter;

		maxAccel = newCharacter.maxAcceleration;
		maxSpeed = newCharacter.maxSpeed;
	}

	//Page 65
	public override Steering GetSteering() {
		Steering returnSteering = new Steering ();

		Vector3 direction = target - character.transform.position;
		float dist = direction.magnitude;

		float targetSpeed;

		if (dist < targetRadius) {
			return returnSteering;
		}

		if (dist > slowRadius) {
			targetSpeed = maxSpeed;
		}

		else {
			targetSpeed = maxSpeed * dist / slowRadius;
		}

		Vector3 targetVelocity = direction;
		targetVelocity = targetVelocity.normalized * targetSpeed;

		returnSteering.linear = (Vector2)targetVelocity - character.GetVelo();
		returnSteering.linear = returnSteering.linear / timeToTarget;

		if (returnSteering.linear.magnitude > maxAccel) {
			returnSteering.linear = returnSteering.linear.normalized * maxAccel;
		}

		return returnSteering;
	}

	public void UpdateTarget(Vector3 newTarget) {
		target = newTarget;
	}

}
