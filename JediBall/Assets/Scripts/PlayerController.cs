using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public GameObject Pins; // Pins object for scoring

	public GameObject player; // for retrieving values from Muse

	public float speed;

	private Rigidbody rb;

	public Text gameOverText;

	public Text startText, alphaText, betaText, connectionText;

	public Button restartButton;

	public Button startButton;

	private bool active = false; // whether ball is active or not

	private bool won = false; // whether player has won

	public bool useAlpha = true; // use alpha or beta waves

	public Rigidbody kinectObject; // object controlled by kinect

	private float TheForceTranslationX; // force on ball by kinect object

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		startText.text = "Welcome to JediBall!";
		gameOverText.text = "";
		restartButton.gameObject.SetActive (false);

		TheForceTranslationX = kinectObject.GetComponent<Rigidbody> ().transform.position.x;
	}

	// count how many pins are down
	public int CheckPins()
	{
		int nStand = 0;
		//See https://docs.unity3d.com/ScriptReference/Transform.html
		foreach (Transform child in Pins.transform) {
			//See http://answers.unity3d.com/questions/1003884/how-to-check-if-an-object-is-upside-down.html
			if (Mathf.Abs (Vector3.Dot (child.up, Vector3.up)) > 0.999f) {
				nStand += 1;
			}
		}
		return Pins.transform.childCount - nStand;
	}

	// transitions to win UI
	public void win()
	{
		int nPin = CheckPins (); // count pins
		startText.text = "Pins Down: " + nPin.ToString();
		restartButton.gameObject.SetActive (true);
		won = true;
	}

	// starts the game
	public void gameStart()
	{
		rb.velocity = new Vector3 (0, 0, 15);
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
		string inUse = " (in use)";
		alphaText.text = useAlpha ? "Alpha: " + alpha_forward + inUse : "Alpha: " + alpha_forward;
		betaText.text = useAlpha ? "Beta: " + beta_forward : "Beta: " + beta_forward + inUse;
		float connection = osc.conn;
		if (connection > 2)
			connectionText.text = "Connection Status: Good";
		else if (connection == 2)
			connectionText.text = "Connection Status: OK";
		else
			connectionText.text = "Connection Status: Bad";
		if (active) {
			if (rb.position.y <= -5.0 && !won) { // game over if ball falls below a certain height
				gameOverText.text = "Game Over!";
				restartButton.gameObject.SetActive (true);
			}
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			if (horizontal > 150 | horizontal < -150) { // threshold for left and right movement
				//				horizontal = 0;
				horizontal = horizontal / 700;
			} else {
				horizontal = 0;
			}
			float forward = 0;

			if (useAlpha) {
				forward = alpha_forward * 2;
			} else {
				forward = beta_forward * 2;
			}

			Vector3 movement = new Vector3 ((moveHorizontal + TheForceTranslationX + horizontal), 0.0f, (moveVertical + forward));
			rb.AddForce (movement * speed);
		} else {
			if (blink > 0)
				useAlpha = !useAlpha;
		}

		if (won) { // updates the pin count after winning
			int nPin = CheckPins ();
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
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); 
    }

	void onGUI()
	{
		if (GUILayout.Button ("Restart")) {
			Restart ();
		}
	}

}