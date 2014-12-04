using UnityEngine;
using System.Collections;

public class AvoidObstacle : Behavior {
	SwarmerController character;

	public float avoidDistance = 1.0f; //the minimum distance to a wall

	public float threshold = 2.0f; //the distance to look ahead for collision
	public float decayCoefficient;
	public float maxAcceleration;

	public AvoidObstacle (SwarmerController character) {
		this.character = character;
		decayCoefficient = 14.0f;
		maxAcceleration = 20.0f;
	}

	public override Steering GetSteering ()
	{
		Steering returnSteering = new Steering();
		Vector2 swarmerPos = character.rigidbody2D.position;
		Vector2 swarmerDir = character.GetDirection();
		swarmerDir.Normalize ();

		RaycastHit2D hit = Physics2D.Raycast (swarmerPos, swarmerDir, threshold, LayerMask.GetMask ("Obstacle"));
//		Debug.DrawRay (swarmerPos, swarmerDir * threshold, Color.red);

		if (hit.collider != null) {
			Vector2 direction = swarmerPos - hit.point;
			float distance = direction.magnitude;
			float strength = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration);
			returnSteering.linear += strength * -direction.normalized;
			
			return returnSteering;
		
//			Vector3 targetPos = hit.point + hit.normal * avoidDistance;
//			Debug.DrawRay(hit.point, hit.normal, Color.blue);
//			//Debug.Log (targetPos);
//			Seek seek = new Seek(targetPos, character);
//			return seek.GetSteering ();
		} else {
			return returnSteering;
		}

	}
}
