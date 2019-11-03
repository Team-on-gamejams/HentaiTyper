using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WordsData", menuName = "Words Data", order = 51)]
public class WordsData : ScriptableObject {
	public List<Word> words;
}

[Serializable]
public class Word {
	public string word;
	public List<Sprite> images;
}
