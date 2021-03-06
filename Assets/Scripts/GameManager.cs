﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    [Header("Music")] [SerializeField] private AudioClip backgroundMusic;

    internal int currentLevel = 0;
    [SerializeField] internal LevelInfo[] LevelInfos;
    [SerializeField] private GameObject[] fires;
    internal bool[] hasBeenOnFired;
    [SerializeField] private int[] maxFireNumOfLevel;
    private int fireLeft;
    private int fireInScene;
    [SerializeField] private float damageSpeed = 1;
    [SerializeField] private float fireSpawnInterval = 8f;
    private float fireSpawnTimeRemain = 0f;

    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject loadingScreen;

    private List<int> firePositions;

    [Header("PlayerData")] internal int[] highestScores;

    [Header("UI")] [SerializeField] private TextMeshProUGUI fireText;

    private int fireNum;
    [SerializeField] private TextMeshProUGUI waterText;
    internal float water = 1;
    [SerializeField] internal float scorePerWaterPercentage = 1000;
    internal bool shootingWater;
    [SerializeField] private float waterLossSpeed = 1f;
    [SerializeField] private TextMeshProUGUI damageText;
    internal float damage = 0;
    [SerializeField] private TextMeshProUGUI timeText;
    internal float minute = 0;
    internal float second = 0;

    internal float PlayerScore = 0;
    [SerializeField] private int basedScore = 100;


    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject VictoryScreen;

    private bool isGameOver;

    private bool gameOver;
    public float scorePerDamagePercentage = 100;

    // Use this for initialization
    void Start() {
        hasBeenOnFired = new bool[14];
        highestScores = new int[LevelInfos.Length];

        SetUpScene();
        fireSpawnTimeRemain = fireSpawnInterval;
        ResetUI();

        if (PlayerPrefs.HasKey("PlayerScore"))
            SetHighestScore(PlayerPrefs.GetString("PlayerScore"));

//        for (int i = 0; i < highestScores.Length; i++) {
//            print("the highest score of level " + (i + 1) + " is " + highestScores[i]);
//        }
    }

    private void SetHighestScore(string password) {
        string[] separatedPlayerInfo = password.Split('/');
        for (int i = 0; i < separatedPlayerInfo.Length; i++) {
            if (string.IsNullOrEmpty(separatedPlayerInfo[i])) continue;    // Skip empty character
            highestScores[i] = Convert.ToInt32(separatedPlayerInfo[i]);
        }
    }

    public void UpdatePlayerDataString() {
        string playerData = string.Empty;
        for (int i = 0; i < highestScores.Length; i++) {
            playerData += highestScores[i] + "/";
        }

        PlayerPrefs.SetString("PlayerScore", playerData);
    }

    private void ResetUI() {
        gameOver = false;
        fireNum = 0;
        water = 100;
        damage = 0;
        minute = 0;
        second = 0;
        PlayerScore = 0;
    }

    private void UpdateUI() {
        UpdateFireText();
        UpdateWaterText();
        UpdateDamageText();
        UpdateTimeText();
    }


    private void UpdateFireText() {
        fireText.text = fireInScene.ToString();
    }

    private void UpdateWaterText() {
        if (Input.GetButton("ShootWater")) {
            water -= LevelInfos[currentLevel].waterLossSpeed * Time.deltaTime;
        }

        waterText.text = (int) water + "%";
    }

    private void UpdateDamageText() {
        damage += fireInScene * LevelInfos[currentLevel].damageSpeed * Time.deltaTime;
        damageText.text = (int) damage + "%";
    }

    private void UpdateTimeText() {
        second += Time.deltaTime;
        if (second >= 60) {
            minute++;
            second -= 60;
        }

        timeText.text = minute + ":" + (int) second;
    }

    // Update is called once per frame
    void Update() {
        fireSpawnTimeRemain -= Time.deltaTime;
        if (fireSpawnTimeRemain <= 0) {
            fireSpawnTimeRemain = fireSpawnInterval;
            RandomlySpawnFire();
        }

        if (!gameOver)
            UpdateUI();
        CheckIfGameOver();

        //        print("Fire in scene: "+fireInScene+". Fire left: "+fireLeft);
        if (Input.GetButtonDown("JumpToNextLevel")) {
            MoveToNextLevel();
        }
    }

    private void CheckIfGameOver() {
        if (damage >= 100 || water <= 0) {
            GameOver();
        }
    }

    public void GameOver() {
        gameOver = true;
        GameOverScreen.SetActive(true);
        foreach (GameObject firePosition in fires) {
            foreach (Transform child in firePosition.transform) {
                Destroy(child.gameObject);
            }
        }

        foreach (bool curWindow in hasBeenOnFired) {
            if (!curWindow) {
                PlayerScore += 500;
            }
        }
    }

    void SetUpScene() {
        firePositions = new List<int>();
//        fireLeft = maxFireNumOfLevel[currentLevel];
        fireLeft = LevelInfos[currentLevel].maxFireNum;
        fireInScene = 0;
        for (int i = 0; i < LevelInfos[currentLevel].startFire; i++) {
            // Randomly spawn two fires at the beginning of the game
            bool result = RandomlySpawnFire();
            if (!result) {
                print("Start fire number is larger than the number of fire limited in the scene");
            }
        }
    }

    bool RandomlySpawnFire() {
        // no empty place to store fire
        if (firePositions.Count >= LevelInfos[currentLevel].FireLimitedInScene || fireLeft <= 0)
            return false;

        int firePosition;
        do {
            firePosition = Random.Range(0, fires.Length);
        } while (firePositions.Contains(firePosition));

        hasBeenOnFired[firePosition] = true;
        GameObject curFire = Instantiate(fire, fires[firePosition].transform.position, Quaternion.identity);
        curFire.transform.SetParent(fires[firePosition].transform);
        curFire.SetActive(true);
        firePositions.Add(firePosition);
        fireLeft--;
        fireInScene++;
        return true;
    }

    public void FireEliminates() {
        fireInScene--;
//        print("FireLeft: "+fireLeft+". Fire in scene "+fireInScene);
        if (fireLeft <= 0 && fireInScene <= 0) {
            MoveToNextLevel();
        }
    }

    void MoveToNextLevel() {
        gameOver = true;

        //        if (second < LevelInfos[currentLevel].AveragePassTime)
        //            PlayerScore += (LevelInfos[currentLevel].AveragePassTime - second) * basedScore;
        foreach (GameObject fireHolder in fires)
        {
            foreach (Transform child in fireHolder.transform)
            {
                Destroy(child.transform.gameObject);
            }
        }
//        StartCoroutine(DestroyAllFire());

        if (Camera.main != null) Camera.main.GetComponent<SimpleMouseRotator>().enabled = false;
        if (currentLevel + 1 >= LevelInfos.Length) {
            VictoryScreen.SetActive(true);

        }
        else {
            loadingScreen.SetActive(true);
        }
    }

    public void GoToNextLevel() {
        loadingScreen.SetActive(false);
        currentLevel++;

            SetUpScene();
            ResetUI();
            if (Camera.main != null) Camera.main.GetComponent<SimpleMouseRotator>().enabled = true;
      
    }



    IEnumerator DestroyAllFire() {
        yield return new WaitForSeconds(2);
        foreach (GameObject fireHolder in fires) {
            foreach (Transform child in fireHolder.transform) {
                Destroy(child.transform.gameObject);
            }
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void RestartCurrentLevel() {
        GoToLevel(currentLevel);
    }

    public void RestartFromBeginning()
    {
        VictoryScreen.SetActive(false);
        currentLevel = 0;

        SetUpScene();
        ResetUI();
        if (Camera.main != null) Camera.main.GetComponent<SimpleMouseRotator>().enabled = true;
    }

    public void GoToLevel(int level)
    {
        currentLevel = level;
        GameOverScreen.SetActive(false);
        SetUpScene();
        ResetUI();
    }

    public void MakeEmpty(GameObject window) {
        for (int i = 0; i < fires.Length; i++) {
            if (fires[i] == window) {
                for (int j = 0; j < firePositions.Count; j++) {
                    if (firePositions[j] == i) {
                        firePositions.RemoveAt(j);
                    }
                }
            }
        }
    }
}