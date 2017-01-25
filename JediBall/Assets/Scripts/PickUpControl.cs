using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpControl : MonoBehaviour {

	// count how many pick ups is in-active
	public int CheckPickUps() {
		int nVanished = 0;
		foreach (Transform child in transform) {
			if (child.CompareTag ("PickUp") && !child.gameObject.activeSelf) {
				nVanished += 1;
			}
		}
		return nVanished;
	}

	public void reinitAllChildren(){
		foreach (Transform child in transform) {
			//child.GetComponent<PickUpScript> ().restart ();
			child.GetComponent<RandomPosition> ().restart ();
		}
	}
}
