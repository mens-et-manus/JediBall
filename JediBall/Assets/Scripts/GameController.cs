// Toshiaki Koike-Akino
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public Text ScoreText;
	private static GameController instance = null;

	public int nInnings = 5;
	private int[] scores;
	private int inning;

	// reset scores
	public void Reset() {
		scores = new int [nInnings];
		for (int i = 0; i < nInnings; i++) {
			scores [i] = 0; // initialized to -1 for not counting
		}
		inning = 0; // reset inning
		ScoreText.text = "";
	}

	// destroy this (for new game)
	public void DestroySelf() {
		Destroy (gameObject);
	}

	// Use this for initialization
	void Awake () {
		// singleton
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
			Reset ();
		} else if (instance != this) {
			DestroySelf ();
		} else {
			DisplayScore ();
		}
			
	}

	// Update Score given new score
	public void UpdateScore(int score) {
		if (inning > nInnings) {
			Reset ();
		}

		scores [inning] = score;
		inning += 1;

		DisplayScore ();
	}

	// Display score
	public void DisplayScore() {
		int total = 0;
		string str = "";
		for (int i = 0; i < inning; i++) {
			str = str + scores [i].ToString () + " ";
			total += scores [i];
		}
		str = str + "= " + total.ToString ();
		ScoreText.text = str;
	}

	void Update() {
		DisplayScore ();
	}
}
