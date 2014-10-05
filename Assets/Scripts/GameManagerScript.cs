using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	/*****************************************/
	/* Public variables                      */
	/*****************************************/

	// Skin for custom fonts
	public GUISkin bettskin;

	// Player names
	public string player1Name = "player1";
	public string player2Name = "player2";

	// Reference to the basketball object
	public BasketballScript basketball;


	/*****************************************/
	/* Private variables                     */
	/*****************************************/

	// Keep the scores of the players
	private int player1Score = 0;
	private int player2Score = 0;
	private string player1ScoreString = "P1: 0";
	private string player2ScoreString = "P2: 0";

	// Dimensions for the score placement
	private int scoreWidth = 100;
	private int scoreHeight = 50;
	private int scoreFromTop = 20;
	private int scoreFromSide = 20;


	/*****************************************/
	/* Public methods                        */
	/*****************************************/

	public void GivePlayerPoint(int playerNum) {
		if (playerNum == 1) {
			player1Score += 1;
		} else if (playerNum == 2) {
			player2Score += 1;
		}
		UpdateScoreStrings ();
	}


	/*****************************************/
	/* Core game methods                     */
	/*****************************************/

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI () {
		GUI.skin = bettskin;
		// left, top, width, height
		GUI.Label (new Rect (scoreFromSide, scoreFromTop,
		                     scoreWidth, scoreHeight),
		           player1ScoreString);
		GUI.Label (new Rect (Screen.width - scoreFromSide - scoreWidth,
		                     scoreFromTop, scoreWidth, scoreHeight),
		           player2ScoreString);
	}


	/*****************************************/
	/* Private methods                       */
	/*****************************************/

	// Update the value of the score strings
	private void UpdateScoreStrings() {
		player1ScoreString = "P1: " + player1Score;
		player2ScoreString = "P2: " + player2Score;
	}


}
