using UnityEngine;
using System.Collections;

public class Align : Behavior {
	GameObject target;
	SwarmerController character;
	
	const float targetRadius = 1.0f;
	const float slowRadius = 10.0f;
	const float timeToTarget = 0.1f;

	public Align (GameObject newTarget, SwarmerController newCharacter) {
		target = newTarget;
		character = newCharacter;
	}

	public static float Remap (float value, float from1, float from2, float to1, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
	
	public static Steering GetRotationSteering(float myRot, float targetRot, SwarmerController character) {
		Steering returnSteering = new Steering();
		
		float maxAngularAccel = character.maxRotationAcceleration;
		float maxRotationSpeed = character.maxRotationSpeed;
		
		float rotation = targetRot - myRot;
		rotation += 360.0f;
		rotation = Mathf.Repeat (rotation, 360.0f);
		if (rotation > 180.0f) {
			rotation -= 360.0f;
		}
		
		float rotationSize = Mathf.Abs (rotation);
		float targetRotation = 0.0f;
		
		if (rotationSize <= targetRadius) {
			returnSteering.stop = true;
			return returnSteering;
		}
		
		if (rotationSize > slowRadius) {
			targetRotation = maxRotationSpeed;
		} else {
			targetRotation = maxRotationSpeed * rotationSize / slowRadius;
		}
		
		targetRotation = targetRotation * Mathf.Sign(rotation);
		returnSteering.angular = targetRotation / timeToTarget;
		
		float angularAcceleration = Mathf.Abs (returnSteering.angular);
		if(angularAcceleration > maxAngularAccel) {
			returnSteering.angular = Mathf.Sign(returnSteering.angular) * maxAngularAccel;
		}
		
		return returnSteering;
	}
	
	//Page 67
	public override Steering GetSteering () {
		if (!target)
			return new Steering();

		float targetRot = target.transform.rotation.eulerAngles.z;
		float myRot = character.transform.rotation.eulerAngles.z;

		return GetRotationSteering(myRot, targetRot, character);
	}
}
