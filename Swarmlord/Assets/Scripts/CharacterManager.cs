using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour {

	GameObject[] players;
	
	private GameObject _currentPlayer;
	public GameObject CurrentPlayer {
		private set {
			if (_currentPlayer != value) {
				if (_currentPlayer)
					_currentPlayer.SendMessage("SetIsControlled", false);
				Debug.Log (value.name);
				if (value)
					value.SendMessage("SetIsControlled", true);
			}
			_currentPlayer = value;
		}
		
		get {
			return _currentPlayer;
		}
	}
	
	public static CharacterManager Instance;

	void Start () {
		Instance = this;
		players = GameObject.FindGameObjectsWithTag("Player");
		
		CurrentPlayer = players[0];
	}
	
	public void SwitchToAlivePlayer() {
		foreach (GameObject player in players) {
			if (player == null)
				continue;
			CharacterController2D controller = player.GetComponent<CharacterController2D>();
			if (controller.Alive) {
				CurrentPlayer = player;
				return;
			}
		}
		
		// TODO: all characters dead, reset level, show game over screen
		Application.LoadLevel(Application.loadedLevel);
	}
	
	void Update () {
		for (int i = 1; i <= players.Length; i++) {
			if (Input.GetKeyDown(i.ToString())) {
				CurrentPlayer = players[i - 1];
				break;
			}
		}
	
	}
}
