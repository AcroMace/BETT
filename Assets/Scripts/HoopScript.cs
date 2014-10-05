using UnityEngine;
using System.Collections;

public class HoopScript : MonoBehaviour {

	/*****************************************/
	/* Public variables                      */
	/*****************************************/

	// Player who can score on this net
	public int playerNum = 1;

	// Reference to the game manager
	public GameManagerScript gm;


	/*****************************************/
	/* Public methods                        */
	/*****************************************/

	public void FlipLocation() {
		Vector2 pos = transform.position;
		transform.position = new Vector2(-pos.x, pos.y);
	}


	/*****************************************/
	/* Core game methods                     */
	/*****************************************/
	
	// Give the player a point if they score
	void OnTriggerEnter2D (Collider2D collided) {
		GameObject player = collided.gameObject;
		if (player.name == PlayerName(playerNum)) {
			if (gm.basketball.GetBallHolder() == playerNum) {
//				print("Player " + playerNum + " scores!");
				gm.GivePlayerPoint(playerNum);
				gm.basketball.Reset();
			}
		}
	}


	/*****************************************/
	/* Private methods                       */
	/*****************************************/

	// Return a player's name given their number
	private string PlayerName(int number) {
		if (number == 1) {
			return "player1";
		} else if (number == 2) {
			return "player2";
		} else {
			print ("ERROR: Invalid player number");
			return "Error";
		}
	}


}
