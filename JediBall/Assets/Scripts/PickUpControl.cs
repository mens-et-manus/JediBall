﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void reinitAllChildren(){
		foreach (Transform child in transform) {
			//child.GetComponent<PickUpScript> ().restart ();
			child.GetComponent<RandomPosition> ().restart ();
		}
	}
}
