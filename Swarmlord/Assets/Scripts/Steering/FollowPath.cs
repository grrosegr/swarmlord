using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FollowPath : Behavior {

	private SwarmerController controller;
	private Arrive arrive;
	
	private IList<Transform> path;
	private int pathIndex;

	public FollowPath(GameObject pathRoot, SwarmerController controller) {
		this.controller = controller;
		this.arrive = new Arrive(Vector2.zero, controller);
		this.pathIndex = 0;	
		
		Enabled = pathRoot != null;
		if (pathRoot)
			this.path = pathRoot.transform.Cast<Transform>().ToList(); // get children
	}

	public override Steering GetSteering() {
		if (path == null || path.Count == 0)
			return new Steering();
		
		Vector2 target = path[pathIndex].position;
		arrive.UpdateTarget(target);
		
		if (Vector2.Distance(controller.transform.position, target) < arrive.targetRadius)
			pathIndex = (pathIndex + 1) % path.Count;
		
		return arrive.GetSteering();
	}
}
