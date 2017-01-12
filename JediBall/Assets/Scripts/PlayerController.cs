using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

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

	}

	public void win()
	{
		startText.text = "You Win!";
		restartButton.gameObject.SetActive (true);
	}

	public void gameStart()
	{
		rb.velocity = new Vector3 (0, 0, 10);
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