using UnityEngine;
using System.Collections;

public class Align : Behavior {
	GameObject target;
	SwarmerController character;
	
	public float maxAngularAccel;
	public float maxRotation;
	
	public float targetRadius = 1.0f;
	public float slowRadius = 10.0f;

	public float timeToTarget = 0.1f;

	public Align (GameObject newTarget, SwarmerController newCharacter) {
		target = newTarget;
		character = newCharacter;
		
		maxAngularAccel = newCharacter.maxAcceleration;
		maxRotation = newCharacter.maxSpeed;
		//maxAngularAccel = 20.0f;
		//maxRotation = 15.0f;
	}

	public static float Remap (float value, float from1, float from2, float to1, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
	//Page 67
	public override Steering GetSteering () {
		Steering returnSteering = new Steering ();

		float targetRot = target.transform.rotation.eulerAngles.z;
		float myRot = character.transform.rotation.eulerAngles.z;

		float rotation = targetRot - myRot;
		rotation += 360.0f;
		rotation = Mathf.Repeat (rotation, 360.0f);
		if (rotation > 180.0f) {
			rotation -= 360.0f;
		}

		float rotationSize = Mathf.Abs (rotation);
		float targetRotation = 0.0f;

		if(rotationSize <= targetRadius) {
			returnSteering.stop = true;
			return returnSteering;
		}

		if(rotationSize > slowRadius) {
			targetRotation = maxRotation;
		}

		else {
			targetRotation = maxRotation * rotationSize / slowRadius;
		}

		targetRotation = targetRotation * Mathf.Sign(rotation);
		returnSteering.angular = targetRotation - character.transform.rotation.z;
		returnSteering.angular = returnSteering.angular / timeToTarget;

		float angularAcceleration = Mathf.Abs (returnSteering.angular);
		if(angularAcceleration > maxAngularAccel) {
			returnSteering.angular = Mathf.Sign(returnSteering.angular) * maxAngularAccel;
		}
		return returnSteering;
	}
}
