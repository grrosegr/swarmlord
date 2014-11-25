using UnityEngine;
using System.Collections;

public class Separate : Behavior {
	GameObject[] targets;
	SwarmerController character;
	
	public float threshold;
	public float decayCoefficient;
	public float maxAcceleration;
	
	public Separate (SwarmerController newCharacter) {
		targets = GameObject.FindGameObjectsWithTag ("swarmer");
		character = newCharacter;
		threshold = 3.0f;
		decayCoefficient = 14.0f;
		maxAcceleration = 20.0f;
	}

	public override Steering GetSteering () {
		//Debug.Log ("Steer!");
		Steering returnSteering = new Steering ();

		Vector3 totalStrength = new Vector3(0,0,0);
		foreach (GameObject swarmer in targets) {
			if(swarmer == character.gameObject) { continue;}
			Vector3 direction = swarmer.transform.position - character.transform.position;
			float distance = direction.magnitude;

			if(distance < threshold) {
				//Debug.Log("THRESHOLD: " + distance);
				float thisStrength = Mathf.Min(decayCoefficient * distance * distance, maxAcceleration);
				direction = direction.normalized;
				totalStrength = totalStrength + (-thisStrength * direction);
			}
			returnSteering.linear = totalStrength;
		}
		return returnSteering;
	}
}
