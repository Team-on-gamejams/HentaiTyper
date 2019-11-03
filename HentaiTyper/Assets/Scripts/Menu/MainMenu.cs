using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MainMenu : MenuBase {
	public void OnPlayClick() {
		MenuManager.TransitTo(MenuManager.GetNeededMenu<GameMenu>());
	}

	public void OnPlayLeftClick() {
		MenuManager.TransitTo(MenuManager.GetNeededMenu<GameMenu>());
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
