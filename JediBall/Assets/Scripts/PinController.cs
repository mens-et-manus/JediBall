// Toshiaki Koike-Akino, 2017 Jan.
using System.Collections;
using UnityEngine;

public class PinController : MonoBehaviour {
	public float scale = 1.0f; // default
	public float sensitivity = 0.999f; // pin down detection sensitivity
	private int nPins;

	// Use this for initialization
	void Start () {
		Reset ();
	}

	// Set Active 10 pins or extra 5 pins if true
	public void ActivatePins(bool extra = false) {
		int i = 0;
		foreach (Transform child in transform) {
			if (child.CompareTag ("Pin")) {		
				i += 1;
				if ((i <= 10 && !extra) || (i>10 && extra)) {
					child.gameObject.SetActive (true);
				}
			}
		}
	}

	// De-activate pins which were knocked down
	public void DeActivateDownPins() {
		foreach (Transform child in transform) {
			if (child.CompareTag ("Pin")) {		
				if (CheckDownPin(child)) {
					child.gameObject.SetActive (false);
				}
			}
		}
	}

	// Reset Pin positions: triangular (not necessarily 10 pins)
	public void Reset () {
		int line = 0; // line from head
		int ith = 0; // i-th pin at line
		nPins = 0;
		foreach (Transform child in transform) {
			if (child.CompareTag("Pin")) {
				//stop motion: http://answers.unity3d.com/questions/12878/how-do-i-zero-out-the-velocity-of-an-object.html
				Rigidbody rb = child.gameObject.GetComponent<Rigidbody> (); // stop motion
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				rb.Sleep ();

				float x = scale	* (ith - 0.5f * line);
				float y = child.localScale.y;
				float z = scale * Mathf.Sqrt (3.0f) * 0.5f * line;
				child.localRotation = Quaternion.identity; // stand up (relative to Pins object)
				child.localPosition = new Vector3 (x, y, z); // re-locate (relative to Pins object)

				ith += 1;
				if (ith > line) {
					ith = 0;
					line += 1;
				}
				nPins += 1;
			}
		}
	}

	// check if pin is down
	private bool CheckDownPin(Transform pin) {
		//See http://answers.unity3d.com/questions/1003884/how-to-check-if-an-object-is-upside-down.html
		return (Vector3.Dot (pin.up, Vector3.up) < sensitivity);
	}

	// count how many pins are down
	public int CheckPins()
	{
		int nDown = 0;
		//See https://docs.unity3d.com/ScriptReference/Transform.html
		foreach (Transform child in transform) {
			if (child.CompareTag ("Pin")) {
				if (CheckDownPin(child)) {
					nDown += 1;
				}
			}
		}
		return nDown;
	}
	
}
