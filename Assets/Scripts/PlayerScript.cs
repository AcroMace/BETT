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


	/*****************************************/
	/* Private variables                     */
	/*****************************************/

	// Velocity to use on button presses
	private float velocityX = 5f;
	private float velocityY = 20f;

	// Store original position to use in reset
	private Vector2 originalPosition;


	/*****************************************/
	/* Private methods                       */
	/*****************************************/

	public void Reset() {
		// Reset position and velocity
		transform.position = originalPosition;
		rigidbody2D.velocity = new Vector2 (0, 0);
	}


	/*****************************************/
	/* Core game methods                   */
	/*****************************************/

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
	}

	// Update for physics
	void FixedUpdate() {
		// Navigation updates
		if (Input.GetButton (GetButtonName("Down"))) {
			rigidbody2D.AddForce (new Vector2(0, -velocityY));
		}
		if (Input.GetButton (GetButtonName("Right"))) {
			rigidbody2D.AddForce (new Vector2(velocityX, 0));
		} else if (Input.GetButton (GetButtonName ("Left"))) {
			rigidbody2D.AddForce (new Vector2(-velocityX, 0));
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
		// Make the player invisible
		renderer.enabled = false;
		// Remove the player from the scene
		transform.position = new Vector2 (-5000, -5000);
		// Wait 5 seconds
		yield return new WaitForSeconds(5);
		Reset ();
		// Make the player visible again
		renderer.enabled = true;
	}

}
