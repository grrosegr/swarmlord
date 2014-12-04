using UnityEngine;
using System.Collections;

public abstract class Behavior {

	private bool _enabled = true;
	public bool Enabled {
		get {
			return _enabled;
		}
		
		protected set {
			_enabled = value;
		}
	}
	
	public abstract Steering GetSteering();

}
