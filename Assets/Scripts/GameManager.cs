using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    private int currentLevel = 0;
    [SerializeField] private LevelInfo[] LevelInfos;
    [SerializeField] private GameObject[] fires;
    [SerializeField] private int[] maxFireNumOfLevel;
    private int fireLeft;
    private int fireInScene;
    [SerializeField] private float damageSpeed = 1;
    [SerializeField] private float fireSpawnInterval = 8f;
    private float fireSpawnTimeRemain = 0f;

    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject loadingScreen;

    private List<int> firePositions;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI fireText;

    private int fireNum;
    [SerializeField] private TextMeshProUGUI waterText;
    private float water = 1;
    internal bool shootingWater;
    [SerializeField] private float waterLossSpeed = 1f;
    [SerializeField] private TextMeshProUGUI damageText;
    private float damage = 0;
    [SerializeField] private TextMeshProUGUI timeText;
    private float minute = 0;
    private float second = 0;

    internal int PlayerScore = 0;

    // Use this for initialization
    void Start() {
        SetUpScene();
        fireSpawnTimeRemain = fireSpawnInterval;
        print("FireLeft: " + fireLeft + ". Fire in scene " + fireInScene);
        ResetUI();
    }

    private void ResetUI() {
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
        if (Input.GetButton("Fire1")) {
            water -= LevelInfos[currentLevel].waterLossSpeed * Time.deltaTime;
        }
        waterText.text = (int)water + "%";
    }

    private void UpdateDamageText() {
        damage += fireInScene * LevelInfos[currentLevel].damageSpeed * Time.deltaTime;
        damageText.text = (int)damage + "%";
    }

    private void UpdateTimeText() {
        second += Time.deltaTime;
        if (second >= 60) {
            minute++;
            second -= 60;
        }
        timeText.text = minute + ":" + (int)second;
    }

    // Update is called once per frame
    void Update() {
        fireSpawnTimeRemain -= Time.deltaTime;
        if (fireSpawnTimeRemain <= 0) {
            fireSpawnTimeRemain = fireSpawnInterval;
            RandomlySpawnFire();
        }
        
        UpdateUI();

//        print("Fire in scene: "+fireInScene+". Fire left: "+fireLeft);
    }

    void SetUpScene() {
        firePositions = new List<int>();
//        fireLeft = maxFireNumOfLevel[currentLevel];
        fireLeft = LevelInfos[currentLevel].maxFireNum;
        fireInScene = 0;
        for (int i = 0; i < LevelInfos[currentLevel].startFire; i++) {
            // Randomly spawn two fires at the beginning of the game
            RandomlySpawnFire();
        }
        
        
    }

    void RandomlySpawnFire() {
        // no empty place to store fire
        if (firePositions.Count >= LevelInfos[currentLevel].FireLimitedInScene || fireLeft <= 0)
            return;

        int firePosition;
        do {
            firePosition = Random.Range(0, fires.Length);
        } while (firePositions.Contains(firePosition));

        GameObject curFire = Instantiate(fire, fires[firePosition].transform.position, Quaternion.identity);
        curFire.transform.SetParent(fires[firePosition].transform);
        curFire.SetActive(true);
        firePositions.Add(firePosition);
        fireLeft--;
        fireInScene++;
    }

    public void FireEliminates() {
        fireInScene--;
//        print("FireLeft: "+fireLeft+". Fire in scene "+fireInScene);
        if (fireLeft <= 0 && fireInScene <= 0) {
            MoveToNextLevel();
        }
    }

    void MoveToNextLevel() {
        StartCoroutine(DestroyAllFire());

        if (Camera.main != null) Camera.main.GetComponent<SimpleMouseRotator>().enabled = false;
        loadingScreen.SetActive(true);
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
}