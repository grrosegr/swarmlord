using UnityEngine;
using System.Collections;

public class Steering {
	public Vector2 linear;
	public float angular;
	public bool stop;

	public Steering() {
		linear = Vector2.zero;
		angular = 0.0f;
		stop = false;
	}
}
