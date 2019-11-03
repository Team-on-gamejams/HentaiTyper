using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
	[SerializeField] byte FirstMenuId;
	[SerializeField] MenuBase[] Menus;
	MenuBase currMenu;

	void Start() {
		currMenu = Menus[FirstMenuId];

		foreach (var menu in Menus) {
			if (menu != currMenu)
				menu.Hide();
			else
				menu.Show();

			menu.MenuManager = this;
		}
	}

	public void TransitTo(MenuBase menu) {
		currMenu.Hide();
		(currMenu = menu).Show();
	}

	public T GetNeededMenu<T>() where T : MenuBase {
		return Menus.First((m)=>m is T) as T;
	}
}
