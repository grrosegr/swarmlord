using UnityEngine;
using System.Collections;

public class Wander : Behavior {

	private SwarmerController controller;

	public Wander(SwarmerController controller) {
		this.controller = controller;
	}

	private static float GetBinomialRandom() {
		// Returns a random number between −1 and 1, where values around zero are more likely.
		return Random.value - Random.value;
	}

	public override Steering GetSteering ()
	{
		Steering steering = new Steering();
		
		steering.angular = GetBinomialRandom() * controller.maxRotationSpeed;
		steering.linear = controller.transform.up * controller.maxSpeed / 50;
		
		return steering;
	}
}
