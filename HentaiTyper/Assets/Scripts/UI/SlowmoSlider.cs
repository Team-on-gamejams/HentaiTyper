using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowmoSlider : MonoBehaviour {
	[SerializeField] Slider[] sliders;

	public void Init(float min, float max) {
		foreach (var slider in sliders) {
			slider.minValue = min;
			slider.maxValue = max;
			slider.value = max;
		}
	}

	public void UpdateValue(float currFill) {
		foreach (var slider in sliders) 
			slider.value = currFill;
	}
}
