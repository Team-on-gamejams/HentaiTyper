using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MenuBase {
	public bool isLeftMode;
	public byte difficulty;

	GameManager gameManager;

	protected override void Awake() {
		base.Awake();

		gameManager = GetComponent<GameManager>();
	}

	protected override void OnEnter() {
		gameManager.StartGame(isLeftMode, difficulty);
	}

	protected override void OnExit() {
		gameManager.StopGame();
	}

	public void OnBackClick() {
		MenuManager.TransitTo(MenuManager.GetNeededMenu<MainMenu>());
	}
}
