using UnityEngine;
using System.Collections;

public abstract class Behavior : MonoBehaviour {

	public Steering GetSteering() {
		return new Steering ();
	}

}
