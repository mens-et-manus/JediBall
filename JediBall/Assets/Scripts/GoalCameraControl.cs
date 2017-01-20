using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GoalCameraControl : MonoBehaviour {

	public GameObject player;
	private Camera camera;

	// Use this for initialization
	void Start () {
		camera = gameObject.GetComponent<Camera>();
		camera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.z > 18f || player.transform.position.y <= -5.0f) {
			camera.enabled = true;
		}
	}
}
