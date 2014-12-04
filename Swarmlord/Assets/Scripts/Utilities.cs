using UnityEngine;
using System.Collections;

public static class Utilities {

	// returns angle of v in radians
	public static float GetAngle(this Vector2 v) {
		return Mathf.Atan2(v.y, v.x);
	}
	
	// angle is in radians
	public static Vector2 GetUnitVector(float angle) {
		return new Vector2(Mathf.Cos (angle), Mathf.Sin (angle));
	}
	
	// angle in radians
	public static Vector2 GetRotated(this Vector2 v, float angle) {
		return GetUnitVector(GetAngle(v) + angle) * v.magnitude;
	}
	
	// angle in degrees
	public static Vector2 GetRotatedByDegrees(this Vector2 v, float angleInDegrees) {
		return v.GetRotated(angleInDegrees * Mathf.Deg2Rad);
	}
}
