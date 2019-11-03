using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovingWord : MonoBehaviour {
	public static float speedMult = 1.0f;

	[NonSerialized] public float speed;

	[SerializeField] TextMeshProUGUI text;

	Word word;
	byte currLetter;
	bool isTyped;

	public void SetWord(Word _word) {
		word = _word;

		currLetter = 0;
		isTyped = false;

		text.text = word.word;
	}

	public bool ProcessChar(char c) {
		if(!isTyped && word.word[currLetter] == c) {
			if(currLetter != word.word.Length - 1) {
				++currLetter;
				text.text = word.word.Substring(currLetter);
			}
			else {
				text.text = "";
				OnTyped();
			}
			return true;
		}
		return false;
	}

	public void ProcessMove() {
		if(!isTyped)
			transform.Translate(-speed * speedMult * Time.deltaTime, 0, 0); 
	}

	void OnTyped() {
		isTyped = true;
	}
}
