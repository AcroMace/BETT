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

	// Rate at which new obstacles spawn in seconds
	public float secPerSpawn = 5f;


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
	// (-4,2) should be at obstacleGrid[0][6]
	private bool[,] obstacleGrid = new bool[8, 8];
	private int obstacleMaxX = 4;
	private int obstacleMaxY = 3;

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
		obstacleGrid = new bool[8, 8];
	}

	public void SpawnObstacle() {
		if (numObstacles < maximumObstacles) {
			// Only spawn if the spot is available
			bool foundAvailableCoordinate = false;
			// (X,Y) coordinates to place the obstacle
			// Scope here for Instantiate
			int obstacleX = 0;
			int obstacleY = 0;
			while (!foundAvailableCoordinate) {
				// 2 possible x-coordinate values for the obstacle
				// Doing this to prevent obstacle spawning right below
				// the ball spawning point
				int obstacleX1 = Random.Range (-obstacleMaxX,-1); // -1: exclusive
				int obstacleX2 = Random.Range (2, obstacleMaxX);  //  2: inclusive
				// Which obstacle X to use
				int obstacleXUse = Random.Range (0, 2); // Returns 0 or 1
				if (obstacleXUse == 1) {
					obstacleX = obstacleX1;
				} else {
					obstacleX = obstacleX2;
				}
				// Y-coordinate
				obstacleY = Random.Range (-obstacleMaxY, obstacleMaxY);
				// Check availability - if no 
				bool obstacleExistsAtCoordinate = (bool)obstacleGrid.GetValue(obstacleX + obstacleMaxX, obstacleY + obstacleMaxY);
				if (!(obstacleExistsAtCoordinate)) {
					foundAvailableCoordinate = true;
					obstacleGrid.SetValue(true, obstacleX + obstacleMaxX, obstacleY + obstacleMaxY);
				}
			}
			Instantiate (obstacle, new Vector2 (obstacleX, obstacleY), Quaternion.identity);
			numObstacles++;
		}
	}


	/*****************************************/
	/* Core game methods                     */
	/*****************************************/

	// Use this for initialization
	void Start () {
		obstacleGrid = new bool[obstacleMaxX * 2, obstacleMaxY * 2];
		// Decrease the max if it's over the amount of points on the grid
		// The '-3' is since the object cannot appear where x = {-1, 0, 1}
		int actualMaxObstacles = (obstacleMaxX * 2 - 3) * (obstacleMaxY * 2 + 1);
		if (maximumObstacles > actualMaxObstacles) {
			print ("Max obstacles too high, decreasing the amount");
			maximumObstacles = actualMaxObstacles;
		}
		InvokeRepeating ("SpawnObstacle", 3, secPerSpawn);
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
