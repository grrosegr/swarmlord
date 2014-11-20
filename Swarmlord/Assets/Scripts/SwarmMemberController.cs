using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SwarmMemberController : MonoBehaviour {

	public float MaxAngleVisible = 45.0f; // degrees
	
	private GameObject[] players;
	
	public GameObject ScreamPrefab;
	public AudioClip ScreamSound;

	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	bool CanSee(GameObject other) {
		Vector2 t_pos = other.transform.position;
		Vector2 m_pos = transform.position;
		Vector2 m_fwd = transform.up;
		Vector2 to_target = t_pos - m_pos;
		float angle = Vector2.Angle(to_target, m_fwd);
		
		if (!(Mathf.Abs(angle) < MaxAngleVisible))
			return false;
		
		RaycastHit2D hit = Physics2D.Raycast(m_pos, to_target, to_target.magnitude, LayerMask.GetMask("Obstacle"));
		
		return hit.collider == null;
	}
	
	private bool screamed = false;
	private float screamResetTime;
	
	public float ScreamResetAfter = 5.0f;
	
	void Scream() {
		if (screamed)
			return;

		//new seek source here

		audio.clip = ScreamSound;
		audio.Stop();
		audio.Play ();
		screamed = true;
		SpriteRenderer r = (SpriteRenderer)renderer;
		r.color = Color.red;
		Instantiate(ScreamPrefab, transform.position, Quaternion.identity);
		screamResetTime = Time.time + ScreamResetAfter;
	}
	
	// Update is called once per frame
	void Update () {
		SpriteRenderer r = (SpriteRenderer)renderer;
		
		if (Time.time > screamResetTime) {
			screamed = false;
			r.color = Color.white;
		}
		
		foreach (GameObject go in players) {
			if (CanSee(go)) {
				Scream();
				GetComponent<SwarmerController> ().AddNewArriveLocation (go.transform.position);
				return;
			}
		}
	}
	
	void OnScream(Vector3 pos) {
		Scream();
	}
}
