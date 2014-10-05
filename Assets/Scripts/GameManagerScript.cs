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

	// Reference to the background
	public Transform background;

	// References to the win texts
	public Transform winScreen1;
	public Transform winScreen2;

	// References to the hoops
	public HoopScript hoop1;
	public HoopScript hoop2;

	// Score needed for a player to win the game
	public int winScore = 5;

	// Maximum number of obstacles on the scene
	public int maximumObstacles = 5;

	// Rate at which new obstacles spawn in seconds
	public float secPerSpawn = 3f;


	/*****************************************/
	/* Private variables                     */
	/*****************************************/

	// Whether or not the game is over
	// Determines whether the player can restart the game
	private bool gameIsOver = false;

	// Keep the scores of the players
	private int[] playerScore = {0,0};
	private string[] playerScoreString = {"P1: 0", "P2: 0"};

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
		if (playerNum == 1 || playerNum == 2) {
			playerScore[playerNum - 1] += 1;
		}
		UpdateScoreStrings();
		if (playerScore[playerNum - 1] == winScore) {
			gameIsOver = true;
			player1.Kill ();
			player2.Kill ();
			player1.Kill ();
			if (playerNum == 1) {
				Instantiate(winScreen1, Vector2.zero, Quaternion.identity);
			} else if (playerNum == 2) {
				Instantiate(winScreen2, Vector2.zero, Quaternion.identity);
			}
		} else {
			FlipScreen();
		}
	}

	// Reset the player, basketball, and scores
	public void Reset() {
		gameIsOver = false;
		// Reset the basketball
		basketball.Reset ();
		// Reset player related variables
		playerScore[0] = 0;
		playerScore[1] = 0;
		UpdateScoreStrings ();
		player1.Reset ();
		player2.Reset ();
		// Delete all obstacles
		numObstacles = 0;
		obstacleGrid = new bool[8, 8];
		// Delete scenary
		DeleteAllObjectsWithTag ("Obstacle");
		DeleteAllObjectsWithTag ("Death");
		DeleteAllObjectsWithTag ("Menu");
	}

	// Spawns an obstacle on a grid if possible
	public void SpawnObstacle() {
		if (!gameIsOver && numObstacles < maximumObstacles) {
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

	// Returns true if game is over
	// Prevents making the variable public
	public bool IsGameOver() {
		return gameIsOver;
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
		if (gameIsOver && Input.GetButtonDown("Confirm")) {
		    Reset ();
		}
	}

	void OnGUI () {
		// Apply the custom skin
		GUI.skin = bettskin;
		// Update player 1's score
		GUI.Label (new Rect (scoreFromSide, scoreFromTop,
		                     scoreWidth, scoreHeight),
		           playerScoreString[0]);
		// Update player 2's score
		GUI.Label (new Rect (Screen.width - scoreFromSide - scoreWidth,
		                     scoreFromTop, scoreWidth, scoreHeight),
		           playerScoreString[1]);
	}


	/*****************************************/
	/* Private methods                       */
	/*****************************************/

	// Update the value of the score strings
	private void UpdateScoreStrings() {
		playerScoreString[0] = "P1: " + playerScore[0];
		playerScoreString[1] = "P2: " + playerScore[1];
	}

	// Delete all objects with a tag
	private void DeleteAllObjectsWithTag(string tag) {
		GameObject[] instances = GameObject.FindGameObjectsWithTag (tag);
		int instancesLength = instances.Length;
		for (int i = 0; i < instancesLength; i++) {
			Destroy(instances[i]);
		}
	}

	// Flips the location of the hoops and the screen
	private void FlipScreen() {
		player1.FlipSpawnPosition();
		player2.FlipSpawnPosition();
		hoop1.FlipLocation();
		hoop2.FlipLocation();
		FlipBackground();
	}

	// Flip the background image horizontally
	private void FlipBackground() {
		Vector2 bgScale = background.transform.localScale;
		background.transform.localScale = new Vector2(-bgScale.x, bgScale.y);
	}


}
