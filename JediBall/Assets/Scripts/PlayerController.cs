using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public GameObject Pins; // Pins object for scoring

	public float speed;

	private Rigidbody rb;

	public Text gameOverText;

	public Text startText;

	public Button restartButton;

	public Button startButton;

	private bool active = false; // whether ball is active or not

	private bool won = false; // whether player has won

	public Rigidbody kinectObject; // object controlled by kinect

	private float TheForceTranslationX; // force on ball by kinect object

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		startText.text = "Welcome to JediBall!";
		gameOverText.text = "";
		restartButton.gameObject.SetActive (false);
		// Call Reset of Pins' script: https://forum.unity3d.com/threads/calling-function-from-other-scripts-c.57072/
		TheForceTranslationX = kinectObject.GetComponent<Rigidbody> ().transform.position.x;

		Pins.GetComponent<PinController> ().Reset();
	}

	// count how many pins are down
	public int CheckPins()
	{
		int nDown = 0; // number of lay downs
		//See https://docs.unity3d.com/ScriptReference/Transform.html
		foreach (Transform child in Pins.transform) {
			//See http://answers.unity3d.com/questions/1003884/how-to-check-if-an-object-is-upside-down.html
			if (Vector3.Dot (child.up, Vector3.up) < 0.90f) {
				nDown += 1;
			}
		}
		return nDown;
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
		if (active) {
			if (rb.position.y <= -5.0 && !won) // game over if ball falls below a certain height
			{
				gameOverText.text = "Game Over!";
				restartButton.gameObject.SetActive(true);
			}
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 ((moveHorizontal + TheForceTranslationX), 0.0f, (moveVertical + 1f));

			rb.AddForce (movement * speed);
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