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

	public GameObject player; 

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
		OSCConnection lr = player.GetComponent<OSCConnection> ();
		float horizontal = lr.acc2;
		if (active) {
			if (rb.position.y <= -5.0 && !won) // game over if ball falls below a certain height
			{
				gameOverText.text = "Game Over!";
				restartButton.gameObject.SetActive(true);
			}
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			if (horizontal > 150 | horizontal < -150) {
				
				horizontal = horizontal / 700;
			} else {
				horizontal = 0;
			}

			Vector3 movement = new Vector3 ((moveHorizontal + TheForceTranslationX + horizontal), 0.0f, (moveVertical + 1f));

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