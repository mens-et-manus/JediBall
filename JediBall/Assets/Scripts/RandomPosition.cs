using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour {
	public float xMin = -2f;
	public float xMax = 2f;
	public float yMin = 0.5f;
	public float yMax = 0.5f;
	public float zMin = -20f;
	public float zMax = 17f;

	public bool xRot = false;
	public bool yRot = true;
	public bool zRot = false;

	public float speed = 0f;

	// Use this for initialization
	void Start () {
		restart ();
	}

	void Update() {
		if (speed > 0f) { // rotating over time
			// rotation
			float x = (xRot) ? 15f : 0f;
			float y = (yRot) ? 30f : 0f;
			float z = (zRot) ? 45f : 0f;
			Vector3 rot = new Vector3 (x, y, z);
			transform.Rotate(rot * Time.deltaTime * speed);
		}

		if (Input.GetKey(KeyCode.R)){
			Start();
		}
	}

	public void restart() {
		// position
		float x = Random.Range (xMin, xMax);
		float y = Random.Range (yMin, yMax);
		float z = Random.Range (zMin, zMax);
		gameObject.transform.position = new Vector3 (x,y,z);

		// rotation
		x = (xRot) ? Random.Range (0f, 180f) : 0f;
		y = (yRot) ? Random.Range (0f, 180f) : 0f;
		z = (zRot) ? Random.Range (0f, 180f) : 0f;
		gameObject.transform.rotation = Quaternion.Euler(x,y,z);	}
}
