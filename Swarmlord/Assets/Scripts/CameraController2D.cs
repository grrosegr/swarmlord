using UnityEngine;
using System.Collections;

public class CameraController2D : MonoBehaviour {

	public float dampTime = 0.15f;
	
	private Vector3 velocity = Vector3.zero;
	private CharacterManager manager;

	// Use this for initialization
	void Start () {
		manager = FindObjectOfType<CharacterManager>();
		transform.position = GetDestination();
	}
	
	private Vector3 GetDestination() {
		GameObject targetObject = manager.CurrentPlayer;
		if (!targetObject)
			return transform.position;
		
		Transform target = targetObject.transform;
		
		Vector3 t_pos = target.position;
		Vector3 point = camera.WorldToViewportPoint(t_pos);
		
		Vector3 cameraCenter = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
		Vector3 delta = t_pos - cameraCenter;
		Vector3 destination = transform.position + delta;
		
		return destination;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.SmoothDamp(transform.position, GetDestination(), ref velocity, dampTime);
	}
}
