using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

	[SerializeField] private Slider progressBar;
	[SerializeField] private GameObject promptText;

	private void Start() {
		LoadLevel(2);
	}

	public void LoadLevel(int sceneIndex) {
		StartCoroutine(LoadAsynchronously(sceneIndex));
	}

	IEnumerator LoadAsynchronously(int sceneIndex) {
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		operation.allowSceneActivation = false;
		while (!operation.isDone) {
			float progress = Mathf.Clamp01(operation.progress / .9f);
			progressBar.value = progress;

			if (progress >= 0.9f) {
				promptText.SetActive(true);
				if (Input.GetKeyDown(KeyCode.Space)) {
					operation.allowSceneActivation = true;
				}
			}
			yield return null;
		}

		
	}
}
