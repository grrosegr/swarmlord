using UnityEngine;
using System.Collections;

public class Wander : Behavior {

	private SwarmerController controller;
	private float changeTime;
	private Vector2 direction;

	public Wander(SwarmerController controller) {
		this.controller = controller;
	}

	private static float GetBinomialRandom() {
		// Returns a random number between −1 and 1, where values around zero are more likely.
		return Random.value - Random.value;
	}
	
	private void NewDirection() {
		direction = Utilities.GetUnitVector(Mathf.PI * 2 * Random.value);
		changeTime = Time.time + Random.Range(5, 10);
	}

	public override Steering GetSteering ()
	{
		Steering steering = new Steering();
		
		if (Time.time > changeTime)
			NewDirection();
		
//		steering.angular = GetBinomialRandom() * controller.maxRotationSpeed;
		steering.linear = direction * controller.maxSpeed / 50;
		
		return steering;
	}
}
