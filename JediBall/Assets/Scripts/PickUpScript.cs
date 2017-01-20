using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour {

	public float xMin = -2f;
	public float xMax = 2f;
	public float yMin = 0.5f;
	public float yMax = 0.5f;
	public float zMin = -20f;
	public float zMax = 17f;

	public GameObject player;

	private string tag = "Player";
	public AudioClip pickupSound;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		restart ();
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.R)) {
			Start ();
		}
	}

	void FixedUpdate (){
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag (tag)) { // player picks up object
			gameObject.GetComponent<MeshRenderer>().enabled = false;
			player.GetComponent<PlayerController> ().pickUpScore += 1;
			if (pickupSound != null) {
				float pitch = Random.Range (0.7f, 1.3f);
				source.pitch = pitch;
				source.PlayOneShot (pickupSound, 1f);
			}
		}
	}

	public void restart(){
		gameObject.GetComponent<MeshRenderer>().enabled = true;
		// position
		float x = Random.Range (xMin, xMax);
		float y = Random.Range (yMin, yMax);
		float z = Random.Range (zMin, zMax);
		gameObject.transform.position = new Vector3 (x,y,z);	}
}
