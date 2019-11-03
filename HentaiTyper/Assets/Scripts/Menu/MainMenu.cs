using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MainMenu : MenuBase {
	public void OnPlayClick() {
		GameMenu gameMenu = MenuManager.GetNeededMenu<GameMenu>();
		gameMenu.isLeftMode = false;
		MenuManager.TransitTo(gameMenu);
	}

	public void OnPlayLeftClick() {
		GameMenu gameMenu = MenuManager.GetNeededMenu<GameMenu>();
		gameMenu.isLeftMode = true;
		MenuManager.TransitTo(gameMenu);
	}

	public void OnCreditsClick() {
		MenuManager.TransitTo(MenuManager.GetNeededMenu<CreditsMenu>());
	}

	public void OnExitClick() {
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
