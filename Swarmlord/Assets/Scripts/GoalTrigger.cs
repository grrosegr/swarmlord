using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoalTrigger : MonoBehaviour {

	IDictionary<GameObject, bool> PlayersWithIntersection; // player -> isCollidng

	private int players;

	// Use this for initialization
	void Start () {
		//renderer.enabled = false; // only render in edit mode
		
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		PlayersWithIntersection = new Dictionary<GameObject, bool>();
		foreach (GameObject player in players) {
			PlayersWithIntersection[player] = false;
		}
	}
	
	
	void Update () {
	}
	
	private void CheckForWin() {
		foreach (bool intersects in PlayersWithIntersection.Values) {
			if (!intersects)
				return;
		}
		
		// You win!
		
		NextLevel();
	}
	
	private void NextLevel()  {
		Application.LoadLevel((Application.loadedLevel + 1) % Application.levelCount);
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			PlayersWithIntersection[other.gameObject] = true;
			
			CheckForWin();
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			PlayersWithIntersection[other.gameObject] = false;
			
			CheckForWin();
		}
	}
}
