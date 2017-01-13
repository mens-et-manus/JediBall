using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public GameObject Pins; // Pins object for scoring

	public float speed;

	private Rigidbody rb;

	public int count;

	public Text gameOverText;

	public Text startText;

	public Button restartButton;

	public Button startButton;

	private bool active = false;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		startText.text = "Welcome to JediBall!";
		gameOverText.text = "";
		restartButton.gameObject.SetActive (false);
		// Call Reset of Pins' script: https://forum.unity3d.com/threads/calling-function-from-other-scripts-c.57072/
		Pins.GetComponent<PinController> ().Reset();
	}

	// count how many pins are down
	public int CheckPins()
	{
		int nStand = 0;
		//See https://docs.unity3d.com/ScriptReference/Transform.html
		foreach (Transform child in Pins.transform) {
			//See http://answers.unity3d.com/questions/1003884/how-to-check-if-an-object-is-upside-down.html
			if (Mathf.Abs (Vector3.Dot (child.up, Vector3.up)) > 0.80f) {
				nStand += 1;
			}
		}
		return Pins.transform.childCount - nStand;
	}

	public void win()
	{
		int nPin = CheckPins (); // count pins
		startText.text = "Down: " + nPin.ToString();
		restartButton.gameObject.SetActive (true);
	}

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
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

			rb.AddForce (movement * speed);
		}

		if (Input.GetKey (KeyCode.Escape)) { // quit application when ESC typed
			Application.Quit ();
		}
		if (Input.GetKey (KeyCode.P)) {
			Pins.GetComponent<PinController> ().Reset();
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Respawn")) {
			gameOverText.text = "Game Over!";
			restartButton.gameObject.SetActive (true);
			//Restart();
		} else if (other.gameObject.CompareTag ("Finish")) {
			win ();
		}
	}

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