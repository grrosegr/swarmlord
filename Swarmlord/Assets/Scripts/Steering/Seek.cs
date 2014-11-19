using UnityEngine;
using System.Collections;

public class Seek : Behavior {

	Vector3 target;
	SwarmerController character;

	float maxAccel = 10.0f;

	public Seek(Vector3 target, SwarmerController character) {
		this.target = target;
		this.character = character;
	}

	public override Steering GetSteering ()
	{
		Steering returnSteering = new Steering ();

		returnSteering.linear = target - character.transform.position;
		returnSteering.linear.Normalize ();

		returnSteering.linear *= maxAccel;
		//Debug.Log (returnSteering.linear);
		returnSteering.angular = 0;

		return returnSteering;
	}

}
