using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
	public static bool IsPaused = false;

	public bool IsPlaying => wordsData != null;

	[SerializeField] MenuManager menuManager;
	[SerializeField] Camera mainCamera;
	[SerializeField] SlowmoSlider slowmoSlider;
	[SerializeField] TextMeshProUGUI scoreText;

	[SerializeField] List<GameDifficulty> difficulties;
	[SerializeField] List<WordsData> wordsDataList;

	[SerializeField] GameObject movingWordPrefab;
	[SerializeField] GameObject flyingImagePrefab;

	GameDifficulty difficulty;
	WordsData wordsData;
	List<MovingWord> movingWords;

	bool isLose;
	int score;

	float currSlowmo;

	float currSpeedMin;
	float currSpeedMax;

	float currTimerMin;
	float currTimerMax;
	float currTimer;
	float elapsedTime;

	void Awake() {
		movingWords = new List<MovingWord>();

		MovingWord.endX = mainCamera.ViewportToWorldPoint(new Vector3(MovingWord.endX, 0)).x;
	}

	void Update() {
		if (!IsPlaying || IsPaused)
			return;

		if((elapsedTime += Time.deltaTime) >= currTimer) 
			LaunchNewWord();

		foreach (MovingWord word in movingWords) 
			word.ProcessMove();

		ProcessSlowmo();
		ProcessInput();
	}

	public void StartGame(bool isLeftMode) {
		IsPaused = false;
		isLose = false;
		scoreText.text = (score = 0).ToString();

		foreach (var word in movingWords) 
			if(word)
				Destroy(word.gameObject);
		movingWords.Clear();

		wordsData = wordsDataList[isLeftMode ? 1 : 0];
		difficulty = difficulties[1];

		currSlowmo = difficulty.slowmoMaxTime;
		slowmoSlider.Init(0, currSlowmo);

		currSpeedMin = difficulty.wordSpeedMin;
		currSpeedMax = difficulty.wordSpeedMax;

		currTimerMin = difficulty.wordTimerMin;
		currTimerMax = difficulty.wordTimerMax;

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
		movingWord.onReachEnd += Lose;
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
		//foreach (char c in Input.inputString) {
		//	bool processThis = false;
		//	if (char.IsLetterOrDigit(c) || c == '-' || c == ' ') {
		//		char cLower = char.ToLower(c);
		//		foreach (MovingWord word in movingWords) {
		//			processThis = word.ProcessChar(cLower);
		//			if (processThis)
		//				break;
		//		}
		//	}
		//	if (processThis)
		//		break;
		//}

		if (movingWords.Count != 0) {
			foreach (char c in Input.inputString) {
				if (char.IsLetterOrDigit(c) || c == '-' || c == ' ') {
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

	void OnWordTyped() {
		GameObject go = Instantiate(
			flyingImagePrefab, 
			mainCamera.ViewportToScreenPoint(new Vector3(Random.Range(0.4f, 0.6f), Random.Range(0.4f, 0.6f))),
			Quaternion.identity,
			transform
		);
		FlyingImage image = go.GetComponent<FlyingImage>();

		image.SetImage(movingWords[0].GetRandomImage(), movingWords[0].transform.position);

		Destroy(movingWords[0].gameObject);
		scoreText.text = (score += movingWords[0].GetScore()).ToString();
		movingWords.RemoveAt(0);
	}

	void Lose() {
		isLose = true;
		IsPaused= true;
		menuManager.TransitTo(menuManager.GetNeededMenu<LoseMenu>(), false);

		foreach (var word in movingWords)
			if (word)
				Destroy(word.gameObject);
	}
}
