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
	private float speedMux; // speed multiplier
	private Vector3 rot;

	// Use this for initialization
	void Start () {
		// position
		float x = Random.Range (xMin, xMax);
		float y = Random.Range (yMin, yMax);
		float z = Random.Range (zMin, zMax);
		gameObject.transform.position = new Vector3 (x,y,z);

		// rotation
		x = (xRot) ? Random.Range (0f, 180f) : 0f;
		y = (yRot) ? Random.Range (0f, 180f) : 0f;
		z = (zRot) ? Random.Range (0f, 180f) : 0f;
		gameObject.transform.rotation = Quaternion.Euler(x,y,z);

		// rotation vector for moving
		rot = new Vector3(x,y,z);
		speedMux = Random.Range (0.1f, 3f);
	}

	void Update() {
		if (speed > 0f) { // rotating over time
			// rotation
			transform.Rotate(rot * Time.deltaTime * speed * speedMux);
		}
	}
	
}
