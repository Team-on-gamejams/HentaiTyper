using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Random = UnityEngine.Random;

public class FlyingImage : MonoBehaviour {
	[SerializeField] Image image;

	[SerializeField] float timeForAnim = 1.0f;

	Vector2 nativeSize;

	public void SetImage(Sprite sprite, Vector3 pos) {
		image.sprite = sprite;
		image.SetNativeSize();

		nativeSize = image.rectTransform.sizeDelta;
		image.rectTransform.sizeDelta /= Random.Range(1f, 3f);
		image.transform.position = pos;
		image.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-25f, 25f));

		Color c = image.color;
		c.a = Random.Range(0.4f, 0.8f);
		image.color = c;

		transform.SetAsFirstSibling();
		StartCoroutine(ShowCoroutine());
	}

	IEnumerator ShowCoroutine() {
		float sizeDelta = nativeSize.x - image.rectTransform.sizeDelta.x;
		float rotateDelta = image.transform.localRotation.eulerAngles.z > 180 ?
			360 - image.transform.localRotation.eulerAngles.z :
			- image.transform.localRotation.eulerAngles.z;
		float alphaDelta = 0.2f;

		float sizeStep = sizeDelta / timeForAnim;
		float rotateStep = rotateDelta / timeForAnim;
		float alphaStep = alphaDelta / timeForAnim;

		while (image.rectTransform.sizeDelta.x < nativeSize.x && image.rectTransform.sizeDelta.y < nativeSize.y) {
			image.rectTransform.sizeDelta += sizeStep * Time.deltaTime * MovingWord.speedMult * Vector2.one;

			image.transform.Rotate(0, 0, rotateStep * Time.deltaTime * MovingWord.speedMult);

			Color c = image.color;
			c.a += alphaStep * Time.deltaTime * MovingWord.speedMult;
			image.color = c;

			yield return null;
		}

		yield return null;
		Destroy(gameObject);
	}
}
