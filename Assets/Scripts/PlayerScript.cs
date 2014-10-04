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

	// Value of the last velocity recorded by FixedUpdate
	Vector2 last_velocity;

	/*****************************************/
	/* Public                                */
	/*****************************************/

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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

		// Store the last velocity to use in OnCollisionEnter2D
		// since the velocity resets to 0 on collision
		last_velocity = rigidbody2D.velocity;
	}

	// Called when colliding with another object with collision detection
	void OnCollisionEnter2D(Collision2D other) {
		// Get the name of the object collided with
		string name = other.gameObject.name;
		// Update the velocity vector on collide
		if (name == "Trampoline") {
			float new_x = last_velocity.x;
			float new_y = last_velocity.y * -1f;
			// Prevent getting stuck on the trampoline
			if (new_y < 5) {
				new_y = 5;
			}
			rigidbody2D.velocity = new Vector2(new_x, new_y);
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
