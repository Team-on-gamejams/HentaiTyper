using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New WordsData", menuName = "Words Data", order = 51)]
public class WordsData : ScriptableObject {
	public List<Word> words;

#if UNITY_EDITOR
	public void ParseWordsData() {
		words = new List<Word>();
		string[] guids = AssetDatabase.FindAssets("t:sprite", new[] { "Assets/Sprites/Manga" });
		foreach (var guid in guids) {
			Sprite art = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid));

			words.Add(new Word {
				word = art.name,
				images = new List<Sprite>() { art }
			});
		}
	}
#endif
}

[Serializable]
public class Word {
	public string word;
	public List<Sprite> images;
}

#if UNITY_EDITOR
[CustomEditor(typeof(WordsData))]
public class WordsDataEditor : Editor {
	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		WordsData data = (WordsData)target;
		if (GUILayout.Button("Parse Sprites/Manga folder")) {
			data.ParseWordsData();
		}
	}
}
#endif