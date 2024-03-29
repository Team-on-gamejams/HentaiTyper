﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameDifficulty", menuName = "Game Difficulty", order = 52)]
public class GameDifficulty : ScriptableObject {
	[Header("Global speed mult")]
	public float startSpeedMult;
	public float slowmoSpeedMult;
	public float slowmoMaxTime;
	public float slowmoRefreshRate;

	[Header("Words speed")]
	public float wordSpeedMin;
	public float wordSpeedMax;
	public float speedAddPerWord;

	[Header("Words timer")]
	public float wordTimerMin;
	public float wordTimerMax;
	public float timerAddPerWord;

	[Header("HP")]
	public float hpMax;
	public float hpRegenPerLetter;
	public float hpLosePerLetter;
}
