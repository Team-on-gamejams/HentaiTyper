using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DifficultiesMenu : MenuBase {
	public void OnEasyClick() {
		GameMenu gameMenu = MenuManager.GetNeededMenu<GameMenu>();
		gameMenu.difficulty = 0;
		MenuManager.TransitTo(gameMenu);
	}

	public void OnNormalClick() {
		GameMenu gameMenu = MenuManager.GetNeededMenu<GameMenu>();
		gameMenu.difficulty = 1;
		MenuManager.TransitTo(gameMenu);
	}

	public void OnHardClick() {
		GameMenu gameMenu = MenuManager.GetNeededMenu<GameMenu>();
		gameMenu.difficulty = 2;
		MenuManager.TransitTo(gameMenu);
	}

	public void OnBackClick() {
		MenuManager.TransitTo(MenuManager.GetNeededMenu<MainMenu>());
	}
}
