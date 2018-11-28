using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private int currentLevel = 0;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private GameObject[] fires;
    [SerializeField] private int[] maxFireNumOfLevel;
    private int fireLeft;
    private int fireInScene;
    [SerializeField] private float fireSpawnInterval = 8f;
    private float fireSpawnTimeRemain = 0f;

    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject loadingScreen;

    private List<int> firePositions;
    // Use this for initialization
    void Start ()
    {
        SetUpScene();
        fireSpawnTimeRemain = fireSpawnInterval;
        print("FireLeft: "+fireLeft+". Fire in scene "+fireInScene);

    }
	
	// Update is called once per frame
	void Update ()
    {
        fireSpawnTimeRemain -= Time.deltaTime;
        if (fireSpawnTimeRemain <= 0) {
            fireSpawnTimeRemain = fireSpawnInterval;
            RandomlySpawnFire();
        }
//        print("Fire in scene: "+fireInScene+". Fire left: "+fireLeft);
    }

    void SetUpScene()
    {
        firePositions = new List<int>();
        fireLeft = maxFireNumOfLevel[currentLevel];
        fireInScene = 0;
        for (int i = 0; i < 2; i++) {   // Randomly spawn two fires at the beginning of the game
            RandomlySpawnFire();
        }
    }

    void RandomlySpawnFire() {
        // no empty place to store fire
        if(firePositions.Count >= 14||fireLeft<=0)
            return;

        int firePosition;
        do
        {
            firePosition = Random.Range(0, fires.Length);
        } while (firePositions.Contains(firePosition));
        GameObject curFire = Instantiate(fire, fires[firePosition].transform.position,Quaternion.identity);
        curFire.SetActive(true);
        firePositions.Add(firePosition);
        fireLeft--;
        fireInScene++;
    }

    public void FireEliminates()
    {
        fireInScene--;
//        print("FireLeft: "+fireLeft+". Fire in scene "+fireInScene);
        if (fireLeft <= 0&&fireInScene<=0)
        {
            MoveToNextLevel();
        }
    }

    void MoveToNextLevel()
    {
        for (int i = 0; i < 14; i++) {
            if (transform.childCount > 0&&!fires[i].transform.GetChild(0).CompareTag("Fire")) {
                Instantiate(fire, fires[i].transform.position,Quaternion.identity);
            }
        }

        if (Camera.main != null) Camera.main.GetComponent<SimpleMouseRotator>().enabled = false;
        loadingScreen.SetActive(true);
    }

    public void GoToNextLevel() {
        loadingScreen.SetActive(false);
        currentLevel++;
        SetUpScene();
        if (Camera.main != null) Camera.main.GetComponent<SimpleMouseRotator>().enabled = true;

    }
}
