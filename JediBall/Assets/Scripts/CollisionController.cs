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

	void OnCollisionEnter(Collision other) {
		float velocity = other.relativeVelocity.magnitude;
		if (other.gameObject.CompareTag (TagName) && velocity > VelocityThreshold) {
			// hit particle
			if (ShockParticle != null) {
				var particle = Instantiate (ShockParticle, other.contacts [0].point, Quaternion.Euler (-90, 0, 0));
				Destroy (particle, 3f);
			}
			// hit sound
			if (ShockSound != null) {
				float vol = 1f - Mathf.Exp (-(velocity - VelocityThreshold) * VolumeSlope);
				source.PlayOneShot (ShockSound, vol);
			}
		}
	}

}
