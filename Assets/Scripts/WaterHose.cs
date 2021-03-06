﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHose : MonoBehaviour {

    private AudioSource audioS;

	// Use this for initialization
	void Start () {
        audioS = GetComponent<AudioSource>();
        audioS.volume = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("ShootWater"))
        {
            audioS.volume = Mathf.Lerp(audioS.volume, 1f, 0.1f);
        }
        else
        {
            audioS.volume = Mathf.Lerp(audioS.volume, 0f, 0.1f);
        }
	}
}
