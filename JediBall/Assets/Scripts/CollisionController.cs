// Toshiaki Koike-Akino, 2017 Jan.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class CollisionController : MonoBehaviour {

	public GameObject ShockParticle; // hit particle
	public AudioClip ShockSound; // hit sound

	public float VelocityThreshold = 2f; // hit decision threshold
	public float VolumeSlope = 0.05f; // sound volume control
	public string TagName = "Player"; // collider tag name

	private AudioSource source;

	// 
	void Start() {
		source = GetComponent<AudioSource> ();
	}

	void RunParticle(Vector3 pos) {
		if (ShockParticle != null) {
			var particle = Instantiate (ShockParticle, pos, Quaternion.Euler (-90, 0, 0));
			Destroy (particle, 3f);
		}
	}

	void RunSound(float vol = 0.7f) {
		if (ShockSound != null) {
			// randomize audio pitch
			float pitch = Random.Range (0.7f, 1.3f);
			source.pitch = pitch;
			source.PlayOneShot (ShockSound, vol);
		}
	}

	void OnCollisionEnter(Collision other) {
		float velocity = other.relativeVelocity.magnitude;
		if (other.gameObject.CompareTag (TagName) && velocity > VelocityThreshold) {
			// hit particle at contact point
			RunParticle(other.contacts [0].point);
			// hit sound: volume is velocity function
			float vol = 1f - Mathf.Exp (-(velocity - VelocityThreshold) * VolumeSlope);
			RunSound (vol);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag (TagName)) {
			// hit particle
			RunParticle (transform.position);
			// hit sound: volume is velocity function
			RunSound (0.9f);
		}
	}

}
