using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GoalCameraControl : MonoBehaviour {

	public GameObject player;
	public float zThreshold = 17f;
	public float yThreshold = -5f;

	private Camera cam;

	// Use this for initialization
	void Start () {
		cam = gameObject.GetComponent<Camera>();
		cam.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.z > zThreshold || player.transform.position.y <= yThreshold) {
			cam.enabled = true;
		}
	}

	public void disableCam() {
		cam.enabled = false;
	}
}
