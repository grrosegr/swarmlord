using UnityEngine;
using System.Collections;

public class VelocityMatch {
	GameObject target;
	SwarmerController character;
	
	public float maxAccel;
	public float maxSpeed;
	
	public float targetRadius = 0.0f;
	public float slowRadius = 10.0f;
	
	public float timeToTarget = 0.01f;
	
	public VelocityMatch(GameObject newTarget, SwarmerController newCharacter) {
		target = newTarget;
		character = newCharacter;
		
		maxAccel = newCharacter.maxAcceleration;
		maxSpeed = newCharacter.maxSpeed;
	}
	
	//Page 65
	public Steering GetSteering() {
		Steering returnSteering = new Steering ();
		
		Vector3 direction = target.transform.position - character.transform.position;
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
		
		returnSteering.linear = targetVelocity - character.velocity;
		returnSteering.linear = returnSteering.linear / timeToTarget;
		
		if (returnSteering.linear.magnitude > maxAccel) {
			returnSteering.linear = returnSteering.linear.normalized * maxAccel;
		}
		
		return returnSteering;
	}
	
	public void UpdateTarget(GameObject newTarget) {
		target = newTarget;
	}
	
}
