using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

	GameObject[] players;
	GameObject currentPlayer;

	// Use this for initialization
	void Start () {
		players = GameObject.FindGameObjectsWithTag("Player");
		
		currentPlayer = players[0];
		currentPlayer.SendMessage("SetIsControlled", true);
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 1; i <= players.Length; i++) {
			if (Input.GetKeyDown(i.ToString())) {
				GameObject oldPlayer = currentPlayer;
				currentPlayer = players[i - 1];
				if (oldPlayer != currentPlayer) {
					oldPlayer.SendMessage("SetIsControlled", false);
					currentPlayer.SendMessage("SetIsControlled", true);
				}
				break;
			}
		}
	
	}
}
