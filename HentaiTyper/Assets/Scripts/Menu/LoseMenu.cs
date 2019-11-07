using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseMenu : MenuBase {
	public void OnBackClick() {
		MenuManager.HideAll();
		MenuManager.TransitTo(MenuManager.GetNeededMenu<MainMenu>());
	}

	protected override void OnEnter() {
		GameManager.IsPaused = true;
	}
}
