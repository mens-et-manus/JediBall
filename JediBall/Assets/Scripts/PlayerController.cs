using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OSCConnection))]
public class PlayerController : MonoBehaviour {
	public GameObject Pins; // Pins object for scoring

	public GameObject player; // for retrieving values from Muse

	public float speedMultiplier;

	public float startingSpeed = 15f;

	private Rigidbody rb;

	public Text gameOverText;

	public Text startText, alphaText, betaText, connectionText, lrText, ScoreText;

	public Button restartButton;

	public Button startButton;

	private bool active = false; // whether ball is active or not

	private bool won = false; // whether player has won

	public bool useAlpha = true; // use alpha or beta waves

	public bool alphaBetaForLR = true; // use alpha/beta waves for left and right movement

	public Rigidbody kinectObject; // object controlled by kinect

	private float TheForceTranslationX; // force on ball by kinect object

	public Image leftArrow, rightArrow;

	public float brainControlThreshold = 0.2f;

	private GameController gameController;

	public void DisplayScores() {
		int inning = gameController.GetInning ();
		string str = "";
		if (inning > 0) {
			int[] scores = gameController.GetScores ();
			for (int i = 0; i < inning; i++) {
				str = string.Format ("{0} {1}", str, scores [i]);
			}
		} 
		else {
			str = string.Format ("High Score: {0}", gameController.GetHighScore ());
		}
		ScoreText.text = str;
	}

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		startText.text = "Welcome to JediBall!";
		gameOverText.text = "";
		restartButton.gameObject.SetActive (false);
		leftArrow.enabled = false;
		rightArrow.enabled = false;

		TheForceTranslationX = kinectObject.GetComponent<Rigidbody> ().transform.position.x;
		Pins.GetComponent<PinController>().Reset (); // Reset Pins

		// Game controller
		// https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/counting-points-and-displaying-score
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
		DisplayScores ();
	}

	// transitions to win UI
	public void win()
	{
		int nPin = Pins.GetComponent<PinController>().CheckPins (); // count pins
		startText.text = "Pins Down: " + nPin.ToString();
		restartButton.gameObject.SetActive (true);
		won = true;
	}

	// starts the game
	public void gameStart()
	{
		rb.velocity = new Vector3 (0, 0, startingSpeed);
		startButton.gameObject.SetActive (false);
		startText.text = "";
		active = true;
	}

	void FixedUpdate ()
	{
		OSCConnection osc = player.GetComponent<OSCConnection> ();
		float horizontal = osc.acc2; // control left and right using gyroscope
		float alpha_forward = osc.alpha; // alpha relative value
		float beta_forward = osc.beta; // beta relative value
		float blink = osc.blink * 5; 
		changeAlphaBetaText (alpha_forward, beta_forward);
		float connection = osc.conn;
		changeConnectionText (connection);
		if (active) {
			if (rb.position.y <= -5.0 && !won) { // game over if ball falls below a certain height
				win();
			}
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			if (!alphaBetaForLR) {
				if (horizontal > 150 | horizontal < -150) { // threshold for left and right movement
					//				horizontal = 0;
					horizontal = horizontal / 700;
					if (horizontal > 0) {
						leftArrow.enabled = false;
						rightArrow.enabled = true;
					} else {
						leftArrow.enabled = true;
						rightArrow.enabled = false;
					}
				} else {
					horizontal = 0;
					leftArrow.enabled = false;
					rightArrow.enabled = false;
				}
			} else {
				if (horizontal > 100 && useAlpha && alpha_forward > brainControlThreshold) {
					horizontal = 1 * alpha_forward;
					rightArrow.enabled = true;
					leftArrow.enabled = false;
				} else if (horizontal > 100 && !useAlpha && beta_forward > brainControlThreshold) {
					horizontal = 1 * beta_forward;
					rightArrow.enabled = true;
					leftArrow.enabled = false;
				} else if (horizontal < -100 && useAlpha && alpha_forward > brainControlThreshold) {
					horizontal = -1 * alpha_forward;
					rightArrow.enabled = false;
					leftArrow.enabled = true;
				} else if (horizontal < -100 && !useAlpha && beta_forward > brainControlThreshold) {
					horizontal = -1 * beta_forward;
					rightArrow.enabled = false;
					leftArrow.enabled = true;
				} else {
					horizontal = 0;
					rightArrow.enabled = false;
					leftArrow.enabled = false;
				}
					
			}
			float forward = 0;

			if (useAlpha) {
				forward = alpha_forward * 2;
			} else {
				forward = beta_forward * 2;
			}

			Vector3 movement = new Vector3 ((moveHorizontal + TheForceTranslationX + horizontal), 0.0f, (moveVertical + forward));
			rb.AddForce (movement * speedMultiplier);
		} else {
			if (blink > 0)
				toggleAlphaBeta();
		}

		if (won) { // updates the pin count after winning
			int nPin = Pins.GetComponent<PinController>().CheckPins ();
			startText.text = "Pins Down: " + nPin.ToString ();
		}

		if (Input.GetKey (KeyCode.Escape)) { // quit application when ESC typed
			Application.Quit ();
		}
	}
		
	void OnTriggerEnter(Collider other) 
	{
//		if (other.gameObject.CompareTag ("Respawn")) {
//			gameOverText.text = "Game Over!";
//			restartButton.gameObject.SetActive (true);
//			//Restart();
//		} else 
		if (other.gameObject.CompareTag ("Finish")) { // player wins when ball hits the end wall
			win ();
		}
	}

	// restarts level
    public void Restart()
    {
		// update score
		int score = Pins.GetComponent<PinController>().CheckPins ();
		gameController.UpdateScore (score);
		// reload scene
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); 
    }

	void onGUI()
	{
		if (GUILayout.Button ("Restart")) {
			Restart ();
		}
	}

	// changes connection status text
	void changeConnectionText(float connection)
	{
		if (connection > 2)
			connectionText.text = "Connection: Good";
		else if (connection == 2)
			connectionText.text = "Connection: OK";
		else
			connectionText.text = "Connection: Bad";
	}

	// changes alpha and beta text
	void changeAlphaBetaText(float alpha_forward, float beta_forward)
	{
		string inUse = " (in use)";
		alphaText.text = useAlpha ? "Alpha: " + alpha_forward + inUse : "Alpha: " + alpha_forward;
		betaText.text = useAlpha ? "Beta: " + beta_forward : "Beta: " + beta_forward + inUse;
	}

	void toggleAlphaBeta()
	{
		useAlpha = !useAlpha;
	}

	public void toggleLRControls()
	{
		alphaBetaForLR = !alphaBetaForLR;
		if (alphaBetaForLR) {
			lrText.text = "Using\nTilt+Brain";
		} else {
			lrText.text = "Using\nTilt Only";
		}
	}
		
}