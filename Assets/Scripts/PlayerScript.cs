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

	// Height to respawn player after death
	public float respawnHeight = 4;


	/*****************************************/
	/* Private variables                     */
	/*****************************************/

	// Velocity to use on button presses
	private float velocityX = 5f;
	private float velocityY = 20f;

	// Store original position to use in reset
	private Vector2 originalPosition;

	// Vector when the player faces right
	private float rightDirection;


	/*****************************************/
	/* Public methods                        */
	/*****************************************/

	public void Reset() {
		// Reset position and velocity
		originalPosition.y = respawnHeight;
		transform.position = originalPosition;
		rigidbody2D.velocity = new Vector2 (0, 0);
	}


	/*****************************************/
	/* Core game methods                   */
	/*****************************************/

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
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
			double other_pos = otherPlayer.transform.position.y;
			double self_pos = transform.position.y;
//			float self_height = renderer.bounds.size.y;
			if (other_pos > self_pos) {
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
		// Wait 5 seconds
		yield return new WaitForSeconds(5);
		Reset ();
		// Make the player visible again
		renderer.enabled = true;
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
