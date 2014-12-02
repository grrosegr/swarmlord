using UnityEngine;
using System.Collections;

public class Nest : MonoBehaviour {

	public GameObject ant;
	public int AntsToSpawn = 200;
	public float SpawnRate = 0.1f;
	
	private int antsSpawned;
	private float lastSpawn;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (antsSpawned < AntsToSpawn && (Time.time - lastSpawn > SpawnRate)) {
			lastSpawn = Time.time;
			antsSpawned += 1;
			Instantiate(ant, transform.position, Quaternion.identity);
		}
	}
}
