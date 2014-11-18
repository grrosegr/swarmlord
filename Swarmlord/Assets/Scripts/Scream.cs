using UnityEngine;
using System.Collections;

public class Scream : MonoBehaviour {

	public float MaxScale = 10.0f;
	public float ScaleSpeed = 0.1f;

	float scale;

	// Use this for initialization
	void Start () {
		scale = 0.0f;
		transform.localScale = new Vector3(scale,scale,scale);
	}
	
	// Update is called once per frame
	void Update () {
		scale += ScaleSpeed;
		transform.localScale = new Vector3(scale,scale,scale);
		
		if (scale > MaxScale)
			Destroy(this);
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "swarmer")
			other.SendMessage("OnScream", transform.position);
	}
}
