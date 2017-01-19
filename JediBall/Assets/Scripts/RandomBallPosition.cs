using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBallPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.transform.position = new Vector3 (Random.Range(-2,2),0.5f,-24f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
