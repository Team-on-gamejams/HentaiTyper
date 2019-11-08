using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MenuBase {
	public void OnBackClick() {
		MenuManager.ShowMenuFromStack();
	}

	public void OnMenuClick() {
		MenuManager.HideAll();
		MenuManager.TransitTo(MenuManager.GetNeededMenu<MainMenu>());
	}

	protected override void OnEnter() {
		GameManager.IsPaused = true;
	}

	protected override void OnExit() {
		GameManager.IsPaused = false;
	}
}
