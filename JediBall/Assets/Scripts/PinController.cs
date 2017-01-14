using System.Collections;
using UnityEngine;

public class PinController : MonoBehaviour {
	public float scale = 1.0f; // default

	// Use this for initialization
	void Start () {
		Reset ();
	}

	// Reset Pin positions: triangular (not necessarily 10 pins)
	public void Reset () {
		int line = 0; // line from head
		int ith = 0; // i-th pin at line
		foreach (Transform child in transform) {
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
		}
	}

	// count how many pins are down
	public int CheckPins()
	{
		int nDown = 0;
		//See https://docs.unity3d.com/ScriptReference/Transform.html
		foreach (Transform child in transform) {
			//See http://answers.unity3d.com/questions/1003884/how-to-check-if-an-object-is-upside-down.html
			if (Vector3.Dot (child.up, Vector3.up) < 0.999f) {
				nDown += 1;
			}
		}
		return nDown;
	}
	
}
