// Toshiaki Koike-Akino, 2017 Jan.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour {
	// singleton 
	// https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/audio-and-sound-manager?playlist=17150
	public static MusicController instance = null;


	// Use this for initialization
	void Awake () {
		// check if not yet instantiated
		if (instance == null) {
			instance = this;
			//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
			DontDestroyOnLoad (gameObject);
		} 
		else if (instance != this) {
			// destroy second one
			Destroy (gameObject);
		}
	}
	
}
