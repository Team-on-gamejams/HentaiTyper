using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public bool IsPlaying => wordsData != null;

	[SerializeField] List<GameDifficulty> difficulties;
	[SerializeField] List<WordsData> wordsDataList;

	GameDifficulty difficulty;
	WordsData wordsData;
	List<Word> movingWords;

	float currMinSpeed;
	float currMaxSpeed;

	void Awake() {
		movingWords = new List<Word>();
	}

	void Update() {
		if (!IsPlaying)
			return;

	}

	public void StartGame(bool isLeftMode) {
		wordsData = wordsDataList[isLeftMode ? 1 : 0];
		difficulty = difficulties[1];

		currMinSpeed = difficulty.wordSpeedMin;
		currMaxSpeed = difficulty.wordSpeedMax;
	}

	public void StopGame() {
		wordsData = null;
	}
}
