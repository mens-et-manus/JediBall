using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.transform.position = new Vector3 (Random.Range(-2,2),0.5f,Random.Range(-20,17));
		gameObject.transform.rotation = Quaternion.Euler(0,Random.Range(0,360),0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
