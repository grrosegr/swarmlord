using UnityEngine;
using System.Collections;

public class FollowObject : Behavior {

	private GameObject target;
	private SwarmerController character;
	private Arrive arrive;

	public FollowObject(GameObject target, SwarmerController character) {
		this.target = target;
		this.character = character;
		this.arrive = new Arrive(Vector2.zero, character);
		Enabled = target != null;
	}
	
	public override Steering GetSteering() {
		Steering steering = new Steering ();
		if (!target)
			return steering;
		
		arrive.UpdateTarget(target.transform.position);
		return arrive.GetSteering();
	}
}
