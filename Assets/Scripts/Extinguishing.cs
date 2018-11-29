using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;
using UnityStandardAssets.Utility;

public class Extinguishing : MonoBehaviour
{
    public float multiplier = 1f;
    [SerializeField] private float reduceFactor = 0.8f;
    [Tooltip("The speed of fire increasing if not extinguished entirely")]
    [SerializeField] private float increaseFactor = 1.2f;
    [SerializeField] private GameObject checkbox;
    [SerializeField] private float TimeWaitForRecover = 1f;
    private GameManager GameManager;
    private ParticleSystemMultiplier ParticleSystemMultiplier;
    private float timePass = 0f;
    private AudioSource audioS;
    private bool isExtinguished;

    // Use this for initialization
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ParticleSystemMultiplier = GetComponent<ParticleSystemMultiplier>();
        checkbox.SetActive(false);
        ParticleSystemMultiplier sysMul = GetComponent<ParticleSystemMultiplier>();
        multiplier = sysMul.multiplier;
        audioS = GetComponent<AudioSource>();
    }

    void Update()
    {
        IncreaseFireWhenNotExtinguished();
    }

    private void IncreaseFireWhenNotExtinguished()
    {
        timePass += Time.deltaTime;
        if (timePass >= 0.2f)
        {
            timePass = 0f;
            FireRecover();
        }
    }

    void Extinguish()
    {
        timePass = 0f;
        multiplier *= reduceFactor;
        audioS.volume -= reduceFactor*2;  // reduce the fire sound
        var systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            ParticleSystem.MainModule mainModule = system.main;
            mainModule.startSizeMultiplier *= reduceFactor;
            mainModule.startSpeedMultiplier *= reduceFactor;
            // mainModule.startLifetimeMultiplier *= Mathf.Lerp(multiplier, 1, 0.5f);
            // system.Clear();
            system.Play();
        }

        if (!isExtinguished && multiplier <= 0.01f) // When the fire is extinguished
        {
            isExtinguished = true;
            GameManager.FireEliminates();
            GetComponent<ParticleSystemDestroyer>().Stop();
            checkbox.SetActive(true);
            checkbox.transform.parent = null;
            audioS.volume = 0;
            GameManager.MakeEmpty(transform.parent.gameObject);
        }
    }

    void FireRecover()
    {
        if (multiplier >= ParticleSystemMultiplier.multiplier)
            return;
        multiplier *= increaseFactor;
        audioS.volume += increaseFactor * 2;
        var systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            ParticleSystem.MainModule mainModule = system.main;
            mainModule.startSizeMultiplier *= increaseFactor;
            mainModule.startSpeedMultiplier *= increaseFactor;
            system.Play();
        }
    }


}