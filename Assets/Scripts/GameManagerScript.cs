using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	/*****************************************/
	/* Public variables                      */
	/*****************************************/

	// Skin for custom fonts
	public GUISkin bettskin;

	// Reference to player objects
	// Used by: none
	public PlayerScript player1;
	public PlayerScript player2;

	// Reference to the basketball object
	// Used by: HoopScript, PlayerScript
	public BasketballScript basketball;

	// Reference to obstacle prefab
	public Transform obstacle;
	
	// Maximum number of obstacles on the scene
	public int maximumObstacles = 5;


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

	// Maximum x and y coordinates for where the obstacle
	// can spawn
	private float obstacleMaxX = 4f;
	private float obstacleMaxY = 4f;

	// Current number of obstacles on the screen
	private int numObstacles = 0;


	/*****************************************/
	/* Public methods                        */
	/*****************************************/

	// Used by PlayerScript when a player dies
	// playerNum should be the number of the player that died
	public void ReleaseBallOnDeath(int playerNum) {
		if (basketball.GetBallHolder() == playerNum) {
			basketball.ReleasePlayer();
		}
	}

	// The player with the playerNum receives a point
	// Called by HoopScript
	// Points are managed solely by the GameManagerScript
	public void GivePlayerPoint(int playerNum) {
		if (playerNum == 1) {
			player1Score += 1;
		} else if (playerNum == 2) {
			player2Score += 1;
		}
		UpdateScoreStrings ();
	}

	// Reset the player, basketball, and scores
	public void Reset() {
		player1Score = 0;
		player2Score = 0;
		UpdateScoreStrings ();
		basketball.Reset ();
		player1.Reset ();
		player2.Reset ();
		// Delete all obstacles
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		for (int i = 0; i < numObstacles; i++) {
			Destroy (obstacles[i]);
		}
		numObstacles = 0;
	}

	public void SpawnObstacle() {
		if (numObstacles < maximumObstacles) {
			float obstacleX = 0;
			// 2 possible x-coordinate values for the obstacle
			// Doing this to prevent obstacle spawning right below
			// the ball spawning point
			float obstacleX1 = Random.Range (-obstacleMaxX,-0.5f);
			float obstacleX2 = Random.Range (0.5f, obstacleMaxX);
			// Which obstacle X to use
			int obstacleXUse = Random.Range (0, 2); // Returns 0 or 1
			if (obstacleXUse == 1) {
				obstacleX = obstacleX1;
			} else {
				obstacleX = obstacleX2;
			}
			// Y-coordinate
			float obstacleY = Random.Range (-obstacleMaxY, obstacleMaxY);
			Instantiate (obstacle, new Vector2 (obstacleX, obstacleY), Quaternion.identity);
			numObstacles++;
		}
	}


	/*****************************************/
	/* Core game methods                     */
	/*****************************************/

	// Use this for initialization
	void Start () {
		InvokeRepeating ("SpawnObstacle", 3, 3f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Reset ();
		}
	}

	void OnGUI () {
		// Apply the custom skin
		GUI.skin = bettskin;
		// Update player 1's score
		GUI.Label (new Rect (scoreFromSide, scoreFromTop,
		                     scoreWidth, scoreHeight),
		           player1ScoreString);
		// Update player 2's score
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
