using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Random = UnityEngine.Random;

public class MovingWord : MonoBehaviour {
	public static float speedMult = 1.0f;
	public static float endX = 0.0f;

	[NonSerialized] public float speed;
	[NonSerialized] public Action onTyped;
	[NonSerialized] public Action onReachEnd;

	[SerializeField] GameObject letterPrefab;

	TextMeshProUGUI[] letters;
	Word word;
	byte currLetter;
	bool isTyped;

	public void SetWord(Word _word) {
		word = _word;

		currLetter = 0;
		isTyped = false;

		letters = new TextMeshProUGUI[word.word.Length];
		for(byte i = 0; i < letters.Length; ++i) {
			letters[i] = Instantiate(letterPrefab, transform.position + new Vector3(i * 52 * transform.lossyScale.x, 0) , Quaternion.identity, transform).GetComponentInChildren<TextMeshProUGUI>();
			letters[i].text = word.word[i].ToString();
		}
	}

	public bool ProcessChar(char c) {
		if(!isTyped && char.ToLower(word.word[currLetter]) == c) {
			if(currLetter != word.word.Length - 1) {
				letters[currLetter].text = "_";
				++currLetter;

				while(letters[currLetter].text == " ") {
					++currLetter;
				}
			}
			else {
				letters[currLetter].text = "_";
				OnTyped();
			}
			return true;
		}
		return false;
	}

	public void ProcessMove() {
		if (!isTyped) {
			transform.Translate(-speed * speedMult * Time.deltaTime, 0, 0);
			if (transform.position.x < endX)
				onReachEnd?.Invoke();
		}
	}

	public Sprite GetRandomImage() {
		return word.images[Random.Range(0, word.images.Count)];
	}

	public int GetScore() {
		return word.word.Length;
	}

	void OnTyped() {
		isTyped = true;
		onTyped?.Invoke();
	}
}
