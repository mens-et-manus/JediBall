using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float speed;

	private Rigidbody rb;

	public int count;

	public Text gameOver;

	public Button restart;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		gameOver.text = "";
		restart.gameObject.SetActive (false);
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag("Respawn")) {
			gameOver.text = "Game Over!";
			restart.gameObject.SetActive (true);
			//Restart();
		}
	}

    void Restart()
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