using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCameraControl : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		Camera camera = gameObject.GetComponent<Camera>();
		camera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		Camera camera = gameObject.GetComponent<Camera>();
		if (player.transform.position.z > 18f || player.transform.position.y <= -5.0f) {
			camera.enabled = true;
		}
	}
}
