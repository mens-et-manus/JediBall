// Toshiaki Koike-Akino
using System.Collections;
using UnityEngine;

// singleton Game Controller to be alive over scenes.
public class GameController : MonoBehaviour {
	public static GameController instance = null;

	public int nInnings = 3;
	private int[] scores = null;
	private int inning = 0;
	private int HighScore = 0;

	// reset scores
	public void Reset() {
		scores = new int [nInnings];
		for (int i = 0; i < nInnings; i++) {
			scores [i] = 0; // initialized to -1 for not counting
		}
		inning = 0; // reset inning

		// high score
		if (PlayerPrefs.HasKey ("HighScore")) {
			HighScore = PlayerPrefs.GetInt ("HighScore");
		}
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
		}	
	}

	public int CalcTotalScore() {
		int TotalScore = 0;
		for (int i = 0; i < inning; i++) {
			TotalScore += scores [i];
		}
		return TotalScore;
	}

	// Update Score given new score
	public void UpdateScore(int score) {
		scores [inning++] = score;
		if (inning >= nInnings) {
			int TotalScore = CalcTotalScore ();
			if (TotalScore > HighScore) { // new high score
				HighScore = TotalScore;
				PlayerPrefs.SetInt ("HighScore", HighScore);
			}
			Reset ();
		}
	}

	// Get
	public int[] GetScores() {
		return scores;
	}

	public int GetInning() {
		return inning;
	}

	public int GetNInnings() {
		return nInnings;
	}

	public int GetHighScore() {
		return HighScore;
	}
}

