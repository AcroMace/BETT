using UnityEngine;
using System.Collections;

public class BounceScript : MonoBehaviour {

	/*****************************************/
	/* Public variables                      */
	/*****************************************/

	// Object to bounce on
	public string bounceTarget = "Trampoline";

	// Minimum vertical velocity
	// Prevents players getting stuck on the trampoline
	public float minimumVelocity = 7f;

	// Multiply the vertical velocity by this factor
	public float bounceFactor = 1f;


	/*****************************************/
	/* Private variables                     */
	/*****************************************/

	// Value of the last velocity recorded by FixedUpdate
	private Vector2 lastVelocity;


	/*****************************************/
	/* Core game methods                     */
	/*****************************************/

	// Update is called once per frame
	void FixedUpdate () {
		// Store the last velocity to use in OnCollisionEnter2D
		// since the velocity resets to 0 on collision
		lastVelocity = rigidbody2D.velocity;
	}


	// Called when colliding with another object with collision detection
	void OnCollisionEnter2D(Collision2D collided) {
		// Get the name of the object collided with
		string name = collided.gameObject.name;

		// Update the velocity vector on collide
		if (name == bounceTarget) {
			float new_x = lastVelocity.x;
			float new_y = lastVelocity.y * -1f * bounceFactor;
			// Prevent getting stuck on the trampoline
			if (new_y < minimumVelocity) {
				new_y = minimumVelocity;
			}
			rigidbody2D.velocity = new Vector2(new_x, new_y);
		}
	}

}
