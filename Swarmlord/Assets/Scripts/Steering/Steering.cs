using UnityEngine;
using System.Collections;

public class Steering {
	public Vector3 linear;
	public float angular;
	public bool stop;

	public Steering() {
		linear = new Vector3(0, 0, 0);
		angular = 0.0f;
		stop = false;
	}
}
