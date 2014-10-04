using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	/*****************************************/
	/* Public                                */
	/*****************************************/

	// Must be 1 or 2
	public int player_num = 1;


	/*****************************************/
	/* Private                               */
	/*****************************************/

	// Velocity to use on button presses
	private float velocity_x = 5f;
	private float velocity_y = 20f;
	

	/*****************************************/
	/* Public                                */
	/*****************************************/

	// Use this for initialization
	void Start () {
	
	}

	// Update for physics
	void FixedUpdate() {
		// Navigation updates
		if (Input.GetButton (get_button_name("Down"))) {
			rigidbody2D.AddForce (new Vector2(0, -velocity_y));
		}
		if (Input.GetButton (get_button_name("Right"))) {
			rigidbody2D.AddForce (new Vector2(velocity_x, 0));
		} else if (Input.GetButton (get_button_name ("Left"))) {
			rigidbody2D.AddForce (new Vector2(-velocity_x, 0));
		}
	}


	/*****************************************/
	/* Private                               */
	/*****************************************/

	// Get the button name given "Up", "Down", "Left", and "Right"
	private string get_button_name(string direction) {
		return "Player" + player_num + "_" + direction;
	}
}
