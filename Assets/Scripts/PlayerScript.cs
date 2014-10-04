using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	/*****************************************/
	/* Public                                */
	/*****************************************/

	// Must be 1 or 2
	public int playerNum = 1;


	/*****************************************/
	/* Private                               */
	/*****************************************/

	// Velocity to use on button presses
	private float velocityX = 5f;
	private float velocityY = 20f;


	/*****************************************/
	/* Public                                */
	/*****************************************/

	// Use this for initialization
	void Start () {
	
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
	

	/*****************************************/
	/* Private                               */
	/*****************************************/

	// Get the button name given "Up", "Down", "Left", and "Right"
	private string GetButtonName(string direction) {
		return "Player" + playerNum + "_" + direction;
	}
}
