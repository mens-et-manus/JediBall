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

	private bool won = false;

	public GameObject Pins;

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
		won = true;
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
            if (rb.position.y <= -5.0 && !won)
            {
                gameOverText.text = "Game Over!";
                restartButton.gameObject.SetActive(true);
            }

            float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical")+ 0.25f;

            Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

			rb.AddForce (movement * speed);
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            win();
        }
        //if (other.gameObject.CompareTag ("Respawn")) {
        //	gameOverText.text = "Game Over!";
        //	restartButton.gameObject.SetActive (true);
        //	//Restart();
        //} else if (other.gameObject.CompareTag ("Finish")) {
        //	win ();
        //}
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

	// check how many pins are not standing
	int CheckPins(){
		int nStand = 0;
		//https://docs.unity3d.com/ScriptReference/Transform.html
		foreach (Transform child in Pins.transform) {
			//http://answers.unity3d.com/questions/1003884/how-to-check-if-an-object-is-upside-down.html
			if (Mathf.Abs (Vector3.Dot (child.up, Vector3.up)) > 0.50f) {
				nStand += 1;
			}
		}
		return Pins.transform.childCount - nStand;
	}

}