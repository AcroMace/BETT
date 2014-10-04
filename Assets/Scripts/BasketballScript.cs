﻿using UnityEngine;
using System.Collections;

public class BasketballScript : MonoBehaviour {

	// Player names
	public string player1Name = "player1";
	public string player2Name = "player2";

	// Number of the player currently holding the ball
	private int ballHolder = 0;

	// True if the ball is currently being held by a player
	private bool playerIsHoldingBall = false;

	// Reference to the player currently holding the ball
	private GameObject playerReference;


	/*****************************************/
	/* Public                                */
	/*****************************************/

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Fix the basketball's position to the player's position
		if (playerIsHoldingBall) {
			transform.position = playerReference.transform.position;
		}
	}

	void OnCollisionEnter2D(Collision2D collided) {
		GameObject player = collided.gameObject;
		// Get reference to the player and enable holding it
		if (IsPlayer(player.name)) {
			playerIsHoldingBall = true;
			ballHolder = PlayerNumber(player.name);
			playerReference = player;
			// Stop weird jerking motions
			collider2D.enabled = false;
		}
	}

	// Prevent client from changing the private value
	public int GetBallHolder() {
		return ballHolder;
	}


	/*****************************************/
	/* Private                               */
	/*****************************************/

	// Return true if the object is a player
	private bool IsPlayer(string name) {
		return (name == player1Name || name == player2Name);
	}

	// Return the player's number given their name
	private int PlayerNumber(string playerName) {
		if (playerName == player1Name) {
			return 1;
		} else if (playerName == player2Name) {
			return 2;
		} else {
			return 0;
		}
	}

}
