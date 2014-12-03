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
	
	int PositiveMod(int x, int m) {
		// To orrect for normally, how, for example
		// -1 % 5 = -1, but we actually want 4
		return (x + m) % m;
	}
	
	void Update () {
		for (int i = 1; i <= players.Length; i++) {
			if (Input.GetKeyDown(i.ToString())) {
				CurrentPlayer = players[i - 1];
				break;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.N)) {
			Application.LoadLevel(PositiveMod(Application.loadedLevel + 1, Application.levelCount));
		} else if (Input.GetKeyDown(KeyCode.P))
			Application.LoadLevel(PositiveMod(Application.loadedLevel - 1, Application.levelCount));
	}
}
