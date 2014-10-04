using UnityEngine;
using System.Collections;

public class HoopScript : MonoBehaviour {

	/*****************************************/
	/* Public variables                      */
	/*****************************************/

	// Player who can score on this net
	public int playerNum = 1;

	// Reference to the basketball
	public BasketballScript basketball;

	// Reference to the game manager
	public GameManagerScript gamemanager;


	/*****************************************/
	/* Core game methods                     */
	/*****************************************/

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Give the player a point if they score
	void OnTriggerEnter2D (Collider2D collided) {
		GameObject player = collided.gameObject;
		if (player.name == PlayerName(playerNum)) {
			if (basketball.GetBallHolder() == playerNum) {
				print("Player " + playerNum + " scores!");
				gamemanager.GivePlayerPoint(playerNum);
				basketball.Reset();
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
