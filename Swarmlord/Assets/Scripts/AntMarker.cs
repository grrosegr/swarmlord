using UnityEngine;
using System.Collections;

public class AntMarker : MonoBehaviour {

	private float startTime;
	public float TimeToLive = 5.0f;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float timeRemaining = (startTime + TimeToLive) - Time.time;
		if (timeRemaining < 0) {
			Destroy(gameObject);
		} else {
			SpriteRenderer sr = (SpriteRenderer)renderer;
			Color color = sr.color;
			color.a = timeRemaining / TimeToLive;
			sr.color = color;
		}
	
	}
}
