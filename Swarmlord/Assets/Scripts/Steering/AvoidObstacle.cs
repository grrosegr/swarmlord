using UnityEngine;
using System.Collections;

public class AvoidObstacle : Behavior {

	//GameObject[] obstacles; //Static obstacles, such as environment, to avoid
	SwarmerController character;

	public float avoidDistance = 30.0f; //the minimum distance to a wall

	public float threshold = 5.0f; //the distance to look ahead for collision

	public AvoidObstacle (SwarmerController character) {
		//obstacles = GameObject.FindGameObjectsWithTag ("Obstacle");
		this.character = character;
	}

	public override Steering GetSteering ()
	{
		Steering returnSteering = new Steering();
		Vector2 swarmerPos = character.transform.position;
		Vector2 swarmerDir = character.transform.up;
		swarmerDir.Normalize ();

		RaycastHit2D hit = Physics2D.Raycast (swarmerPos, swarmerDir, threshold, LayerMask.GetMask ("Obstacle"));
		Debug.DrawRay (swarmerPos, swarmerDir * threshold, Color.red);

		if (hit.collider != null) {
			Vector3 targetPos = hit.point + hit.normal * avoidDistance;
			//Debug.Log (targetPos);
			Seek seek = new Seek(targetPos, character);
			return seek.GetSteering ();
		} else {
			return returnSteering;
		}

	}
}
