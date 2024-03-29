﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
	public static bool IsPaused = false;

	public bool IsPlaying => wordsData != null;

	[SerializeField] MenuManager menuManager;
	[SerializeField] Camera mainCamera;
	[SerializeField] SlowmoSlider slowmoSlider;
	[SerializeField] HpSlider hpSlider;
	[SerializeField] TextMeshProUGUI scoreText;

	[SerializeField] List<GameDifficulty> difficulties;
	[SerializeField] List<WordsData> wordsDataList;

	[SerializeField] GameObject movingWordPrefab;
	[SerializeField] GameObject flyingImagePrefab;
	[SerializeField] Transform imagesParent;

	GameDifficulty difficulty;
	WordsData wordsData;
	List<MovingWord> movingWords;
	List<FlyingImage> images;

	bool isLose;
	int score;

	float currSlowmo;

	float currSpeedMin;
	float currSpeedMax;

	float currTimerMin;
	float currTimerMax;
	float currTimer;
	float elapsedTime;

	float maxHp;
	float currHp;

	void Awake() {
		movingWords = new List<MovingWord>();
		images = new List<FlyingImage>();

		MovingWord.endX = mainCamera.ViewportToWorldPoint(new Vector3(MovingWord.endX, 0)).x;
	}

	void Update() {
		if (!IsPlaying || IsPaused)
			return;

		if(movingWords.Count == 0 && (elapsedTime += Time.deltaTime) >= currTimer) 
			LaunchNewWord();

		for(byte i = 0; i < movingWords.Count; ++i) {
			movingWords[i].ProcessMove();
			if(movingWords[i] == null) {
				movingWords.RemoveAt(i);
				--i;
			}
		}

		ProcessSlowmo();
		ProcessInput();
	}

	public void StartGame(bool isLeftMode, byte difficultyLevel) {
		IsPaused = false;
		isLose = false;
		scoreText.text = (score = 0).ToString();

		foreach (var word in movingWords) 
			if(word)
				Destroy(word.gameObject);
		movingWords.Clear();

		foreach (var image in images)
			if (image)
				Destroy(image.gameObject);
		images.Clear();

		wordsData = wordsDataList[isLeftMode ? 1 : 0];
		difficulty = difficulties[difficultyLevel];

		currSlowmo = difficulty.slowmoMaxTime;
		slowmoSlider.Init(0, currSlowmo);

		currSpeedMin = difficulty.wordSpeedMin;
		currSpeedMax = difficulty.wordSpeedMax;

		currTimerMin = difficulty.wordTimerMin;
		currTimerMax = difficulty.wordTimerMax;

		currHp = maxHp = difficulty.hpMax;
		hpSlider.Init(0, maxHp);

		elapsedTime = 0;
		currTimer = Random.Range(currTimerMin, currTimerMax);
	}

	public void StopGame() {
		wordsData = null;
	}

	void LaunchNewWord() {
		elapsedTime -= currTimer;
		currTimerMin += difficulty.timerAddPerWord;
		currTimerMax += difficulty.timerAddPerWord;
		currTimer = Random.Range(currTimerMin, currTimerMax);

		GameObject go = Instantiate(movingWordPrefab, mainCamera.ViewportToScreenPoint(new Vector3(1.0f, Random.Range(0.2f, 0.8f))), Quaternion.identity, transform);
		MovingWord movingWord = go.GetComponent<MovingWord>();

		movingWord.speed = Random.Range(currSpeedMin, currSpeedMax);
		movingWord.SetWord(RandomEx.GetRandom(wordsData.words));
		movingWord.onReachEnd += GetDamage;
		movingWord.onTyped += OnWordTyped;

		movingWords.Add(movingWord);

		currSpeedMin += difficulty.speedAddPerWord;
		currSpeedMax += difficulty.speedAddPerWord;
	}

	void ProcessSlowmo() {
		if (Input.GetKey(KeyCode.Space)) {
			if (currSlowmo > 0) {
				MovingWord.speedMult = difficulty.slowmoSpeedMult;
				currSlowmo -= Time.deltaTime;
				if (currSlowmo < 0)
					currSlowmo = 0;
				slowmoSlider.UpdateValue(currSlowmo);
			}
			else {
				MovingWord.speedMult = difficulty.startSpeedMult;
			}
		}
		else {
			MovingWord.speedMult = difficulty.startSpeedMult;

			if (currSlowmo < difficulty.slowmoMaxTime) {
				currSlowmo += Time.deltaTime * difficulty.slowmoRefreshRate;
				if (currSlowmo > difficulty.slowmoMaxTime)
					currSlowmo = difficulty.slowmoMaxTime;
				slowmoSlider.UpdateValue(currSlowmo);
			}
		}
	}

	void ProcessInput() {
		if (movingWords.Count != 0) {
			foreach (char c in Input.inputString) {
				if (char.IsLetterOrDigit(c) || c == '-' || c == ' ' || c == '!' || c == '?' || c == ':' || c == '.' || c == ',') {
					char cLower = char.ToLower(c);
					movingWords[0].ProcessChar(cLower);
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape) && !isLose) {
			if (IsPaused) {
				menuManager.ShowMenuFromStack();
			}
			else {
				menuManager.TransitTo(menuManager.GetNeededMenu<PauseMenu>(), false);
			}
		}
	}

	void OnWordTyped(byte typedLetters) {
		GameObject go = Instantiate(
			flyingImagePrefab, 
			mainCamera.ViewportToScreenPoint(new Vector3(Random.Range(0.3f, 0.7f), Random.Range(0.3f, 0.7f))),
			Quaternion.identity,
			imagesParent
		);
		FlyingImage image = go.GetComponent<FlyingImage>();

		image.SetImage(movingWords[0].GetRandomImage(), go.transform.position);
		image.transform.SetAsLastSibling();
		images.Add(image);

		Destroy(movingWords[0].gameObject);
		scoreText.text = (score += movingWords[0].GetScore()).ToString();
		movingWords.RemoveAt(0);

		if ((currHp += typedLetters * difficulty.hpRegenPerLetter) > maxHp) 
			currHp = maxHp;
		hpSlider.UpdateValue(currHp);
	}

	void GetDamage(byte missedLetter) {
		currHp -= missedLetter * difficulty.hpLosePerLetter;
		hpSlider.UpdateValue(currHp);
		if (currHp <= 0) {
			Lose();
		}
		else {
			Destroy(movingWords[0].gameObject);
		}
	}

	void Lose() {
		isLose = true;
		IsPaused= true;
		menuManager.TransitTo(menuManager.GetNeededMenu<LoseMenu>(), false);

		foreach (var word in movingWords)
			if (word)
				Destroy(word.gameObject);
		foreach (var image in images)
			if (image)
				Destroy(image.gameObject);
	}
}
