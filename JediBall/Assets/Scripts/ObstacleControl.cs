﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void reinitAllChildren() {
		foreach (Transform child in transform) {
			child.GetComponent<RandomPosition> ().restart ();
		}
	}
}
