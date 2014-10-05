using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	/*****************************************/
	/* Public variables                      */
	/*****************************************/

	// Must be 1 or 2
	public int playerNum = 1;

	// Reference to the other player
	public PlayerScript otherPlayer;

	// Reference to the game manager
	public GameManagerScript gm;

	// Reference to the death prefab
	public Transform death;

	// Reference to the egg shatter animation prefab
	public Transform shatter;

	// Direction that the player is facing
	// False if the player is facing left
	public bool facingRight = true;

	// Respawn time in seconds
	public int respawnTime = 3;

	// Height to respawn player after death
	public float respawnHeight = 4;


	/*****************************************/
	/* Private variables                     */
	/*****************************************/

	// Velocity to use on button presses
	private float velocityX = 5f;
	private float velocityY = 20f;

	// Store original position to use in reset
	private Vector2 spawnPosition;

	// Vector when the player faces right
	private float rightDirection;


	/*****************************************/
	/* Public methods                        */
	/*****************************************/

	// Put player back to where it originally spawns
	public void Reset() {
		// Reset position and velocity
		spawnPosition.y = respawnHeight;
		transform.position = spawnPosition;
		rigidbody2D.velocity = new Vector2 (0, 0);
		// Make the player visible again
		renderer.enabled = true;
	}

	// Horizontal flip of where the player spawns
	public void FlipSpawnPosition() {
		spawnPosition = new Vector2(-spawnPosition.x, spawnPosition.y);
	}

	// Kill player and remove them from the scene
	public void Kill() {
		gm.ReleaseBallOnDeath(playerNum);
		float xCoord = transform.position.x;
		float yCoord = transform.position.y;
		// Make the player invisible
		renderer.enabled = false;
		// Splatter the egg with random rotation
		Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
		Instantiate (death, new Vector2 (xCoord, yCoord), rotation);
		// Explode facing downwards
		Quaternion downwards = Quaternion.Euler (90, 0, 0);
		Instantiate (shatter, new Vector2 (xCoord, yCoord - 0.5f), downwards);
		// Remove the player from the scene
		transform.position = new Vector2 (-5000, -5000);
	}


	/*****************************************/
	/* Core game methods                   */
	/*****************************************/

	// Use this for initialization
	void Start () {
		spawnPosition = transform.position;
		rightDirection = transform.localScale.x;
		if(!facingRight) {
			TurnLeft();
		}
		// Put the particle system in the front
		shatter.particleSystem.renderer.sortingLayerName = "Shatter";
	}


	// Update for physics
	void FixedUpdate() {
		// Navigation updates
		if (Input.GetButton (GetButtonName("Down"))) {
			rigidbody2D.AddForce (new Vector2(0, -velocityY));
		}
		if (Input.GetButton (GetButtonName("Right"))) {
			rigidbody2D.AddForce (new Vector2(velocityX, 0));
			TurnRight();
		} else if (Input.GetButton (GetButtonName ("Left"))) {
			rigidbody2D.AddForce (new Vector2(-velocityX, 0));
			TurnLeft();
		}
	}

	void OnCollisionEnter2D(Collision2D collided) {
		// If collided with other player, and the other player
		// is above you, then respawn
		if (collided.gameObject.name == otherPlayer.name) {
			// Current height of players
			double other_pos = otherPlayer.transform.position.y;
			double self_pos = transform.position.y;
			// Negative of the velocities of players
			// More negative means likelier to kill other player
			double other_vel = -otherPlayer.rigidbody2D.velocity.y;
			double self_vel = -rigidbody2D.velocity.y;
			if (other_pos * other_vel > self_pos * self_vel) {
				StartCoroutine("Respawn");
			}
		} else if (collided.gameObject.tag == "Obstacle") {
			StartCoroutine("Respawn");
		}
	}
	

	/*****************************************/
	/* Private methods                       */
	/*****************************************/

	// Get the button name given "Up", "Down", "Left", and "Right"
	private string GetButtonName(string direction) {
		return "Player" + playerNum + "_" + direction;
	}

	// Time out the player for 5 seconds
	private IEnumerator Respawn() {
		Kill ();
		// Wait 5 seconds
		yield return new WaitForSeconds(respawnTime);
		// Only reset if the player is dead
		// Prevents glitching if the game is reset while
		// a player is dead
		if (!renderer.enabled) {
			Reset ();
		}
	}
	

	// Face direction of movement
	private void TurnRight() {
		facingRight = true;
		transform.localScale = new Vector2(rightDirection, transform.localScale.y);
	}
	private void TurnLeft() {
		facingRight = false;
		transform.localScale = new Vector2(-rightDirection, transform.localScale.y);
	}

}
