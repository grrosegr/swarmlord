using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlendedSteering {
	
	public float maxAccel; //Maximum acceleration
	public float maxRot; //Maximum rotation

	SwarmerController character;

	//Struct that holds a behavior and that behavior's weight.
	public struct BehaviorWeight {
		public Behavior behavior;
		public float weight;
	};

	private List<BehaviorWeight> behaviors;

	//Constructor
	public BlendedSteering(SwarmerController character) {
		this.character = character;

		maxAccel = character.maxAcceleration;
		maxRot = character.maxRotationAcceleration;

		behaviors = new List<BehaviorWeight> ();
	}

	public void ResetList () {
		behaviors = new List<BehaviorWeight> ();
	}

	//Adds a new behavior with a corresponding weight to the 
	//list of behaviors.
	public void AddBehavior(Behavior behavior, float weight) {
		BehaviorWeight bw = new BehaviorWeight ();
		bw.behavior = behavior;
		bw.weight = weight;
		behaviors.Add (bw);
	}

	//Returns the blended steering behavior.
	public Steering GetSteering() {
		//Create the new steering structure to accumulate the combined steering.
		Steering steering = new Steering ();
		
		var totalWeight = //1.0f;
			behaviors
			.Where(behaviorWeight => behaviorWeight.behavior.Enabled)
			.Select(behaviorWeight => behaviorWeight.weight)
			.Sum();
			
		if (totalWeight <= 0) {
			//Debug.LogError("Behavior weights sum to <= 0!");
			return steering;
		}

		foreach (BehaviorWeight b in behaviors) {
			Steering bSteering = b.behavior.GetSteering ();
			float weight = b.weight / totalWeight;

			//Accumulate weighted steering components in steering
			steering.linear += weight * bSteering.linear;
			steering.angular += weight * bSteering.angular;

			if (bSteering.stop && weight > 0) {
				steering.stop = true;
			}
		}

		//Crop result according to maxAccel and maxRot
		if (steering.linear.magnitude > maxAccel) {
			steering.linear = steering.linear.normalized * maxAccel;
		} 

		if(steering.angular > maxRot) {
			steering.angular = maxRot;
		}

		return steering;
	}

}
