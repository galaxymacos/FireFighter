using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {

	[SerializeField] private float lifeTime = 3f;
	[SerializeField] private TextMeshProUGUI loadingText;
	[SerializeField] private GameObject continueButton;
	private bool loadingFinished;

	private float lifeTimeRemain;
	// Use this for initialization
	void Start () {
		lifeTimeRemain = lifeTime;
	}
	
	// Update is called once per frame
	void Update () {
		lifeTimeRemain -= Time.deltaTime;
		if (!loadingFinished&&lifeTimeRemain <= 0) {
			loadingFinished = true;
			loadingText.transform.localScale = new Vector3(1,1,1);
			loadingText.GetComponent<Animator>().enabled = false;
			loadingText.text = "Finished";
			continueButton.SetActive(true);			
		}
	}
}
