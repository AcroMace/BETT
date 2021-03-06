﻿using UnityEngine;
using System.Collections;

public class BasketballScript : MonoBehaviour {

	/*****************************************/
	/* Public variables                      */
	/*****************************************/

	// Player names
	public string player1Name = "player1";
	public string player2Name = "player2";
	
	// Max force applied to ball when hitting an obstacle
	public float maxObstacleForceX = 5f;
	public float maxObstacleForceY = 2f;


	/*****************************************/
	/* Private variables                     */
	/*****************************************/

	// True if the ball is currently being held by a player
	private bool playerIsHoldingBall = false;

	// Number of the player currently holding the ball
	private int ballHolder = 0;

	// Reference to the player currently holding the ball
	private GameObject playerReference;


	/*****************************************/
	/* Public methods                        */
	/*****************************************/
	
	// Prevent client from changing the private value
	public int GetBallHolder() {
		return ballHolder;
	}
	
	// Reset the ball to its original position/velocity
	public void Reset() {
		// Reset position
		Vector2 originalPosition = new Vector2 (0, 2);
		transform.position = originalPosition;
		// Release player
		ReleasePlayer();
	}

	// Stop following the player and re-enable collision
	// Called when the player dies
	public void ReleasePlayer() {
		// Reset the velocity
		rigidbody2D.velocity = new Vector2 (0, 0);
		// Re-enable collision detection
		collider2D.enabled = true;
		// Reset initial variables related to the player
		ballHolder = 0;
		playerIsHoldingBall = false;
		playerReference = null;
	}



	/*****************************************/
	/* Core game methods                     */
	/*****************************************/

	// Update is called once per frame
	void Update () {
		// Fix the basketball's position to the player's position
		if (playerIsHoldingBall) {
			Vector2 playerPosition = playerReference.transform.position;
			if (playerReference.transform.localScale.x > 0) {
				playerPosition.x += 0.2f;
			} else {
				playerPosition.x -= 0.2f;
			}
			transform.position = playerPosition;
		}
	}

	void OnCollisionEnter2D(Collision2D collided) {
		GameObject player = collided.gameObject;
		// Get reference to the player and enable holding it
		if (player.tag == "Player") {
			playerIsHoldingBall = true;
			ballHolder = PlayerNumber(player.name);
			playerReference = player;
			// Stop weird jerking motions
			collider2D.enabled = false;
		} else if (player.tag == "Obstacle") {
			float forceX = Random.Range(-maxObstacleForceX, maxObstacleForceX);
			float forceY = Random.Range (-maxObstacleForceY, maxObstacleForceY);
			Vector2 randForce = new Vector2(forceX, forceY);
			rigidbody2D.AddForce(randForce);
		}
	}


	/*****************************************/
	/* Private methods                       */
	/*****************************************/

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
