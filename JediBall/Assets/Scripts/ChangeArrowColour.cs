using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeArrowColour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Image> ().color = new Color(9f,224f,244f);
	}
	
	// Update is called once per frame
	void Update () {
//		gameObject.GetComponent<Image> ().color = new Color(9f,224f,244f);
	}
}
