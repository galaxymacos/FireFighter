using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour {

	[SerializeField] private Slider musicSlider;
	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider graphicsSlider;

	[SerializeField] private AudioMixer AudioMixer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeMusicVolume(float volume) {
		AudioMixer.SetFloat("MusicVol", volume);
	}

	public void ChangeSfxVolume(float volume) {
		AudioMixer.SetFloat("SFXVol", volume);
	}

	public void ChangeQualitySetting(float qualityLevel) {
		QualitySettings.SetQualityLevel((int)qualityLevel);
	}
}
