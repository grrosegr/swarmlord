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
		Steering returnSteering = new Steering ();

		foreach (GameObject swarmer in targets) {
			if (swarmer == character.gameObject) continue;
			Vector2 direction = swarmer.transform.position - character.transform.position;
			float distance = direction.magnitude;

			if(distance < threshold) {
				float strength = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration);
				returnSteering.linear += strength * -direction.normalized;
			}
		}
		
		return returnSteering;
	}
}
