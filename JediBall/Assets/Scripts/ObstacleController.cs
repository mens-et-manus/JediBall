using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour {

	public GameObject ShockParticle;

	// 
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag ("Player")) {
			Instantiate (ShockParticle, transform.position, Quaternion.Euler(-90,0,0));
			Destroy (ShockParticle, 5f);
		}
	}

}
