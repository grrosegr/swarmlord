using UnityEngine;
using System.Collections;

public class VelocityMatch : Behavior {
	
	SwarmerController character;
	
	public float maxAccel;
	public float maxSpeed;
	
	public float targetRadius = 0.0f;
	public float slowRadius = 10.0f;
	
	public float timeToTarget = 0.01f;
	
	public VelocityMatch(SwarmerController newCharacter) {
		character = newCharacter;
		
		maxAccel = newCharacter.maxAcceleration;
		maxSpeed = newCharacter.maxSpeed;
	}
	
	//Page 70
	public override Steering GetSteering() {
		Steering steering = new Steering ();
		
		GameObject target = character.myTarget;
		Enabled = target != null;
		if (!target)
			return steering;
		
		steering.linear = target.rigidbody2D.velocity - character.GetVelo();
		steering.linear /= timeToTarget;
		
		if (steering.linear.magnitude > maxAccel)
			steering.linear = steering.linear.normalized * maxAccel;
			
		return steering;
	}	
}
