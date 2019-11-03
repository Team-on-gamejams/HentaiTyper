using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MenuBase {
	public void OnBackClick() {
		MenuManager.TransitTo(MenuManager.GetNeededMenu<MainMenu>());
	}
}
