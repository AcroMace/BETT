using UnityEngine;
using System.Collections;

public class HoopScript : MonoBehaviour {

	// Player who can score on this net
	public int playerNum = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Give the player a point if they score
	void OnCollisionEnter2D (Collision2D collided) {
		string name = collided.gameObject.name;
		if (name == "player1" && playerNum == 1) {
			print("Player 1 scores!");
		} else if (name == "player2" && playerNum == 2) {
			print("Player 2 scores!");
		}
	}

}
