using UnityEngine;
using System.Collections;

public class StayAtHome : Behavior {
	
	const float DistanceThreshold = 3.0f;
	private Vector2 home;
	SwarmerController controller;
	private Arrive arrive;

	public StayAtHome(Vector2 home, SwarmerController controller) {
		this.home = home;
		this.controller = controller;
		this.arrive = new Arrive(home, controller);
	}

	public override Steering GetSteering() {
		if (Vector2.Distance(controller.transform.position, home) > DistanceThreshold) {
			return arrive.GetSteering();
		}
		
		return new Steering();
	}
}
