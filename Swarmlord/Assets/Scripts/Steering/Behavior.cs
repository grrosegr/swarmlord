using UnityEngine;
using System.Collections;

public abstract class Behavior {

	public abstract Steering GetSteering();
	/*
	public abstract Steering GetSteering() {
		return new Steering ();
	}*/

}
