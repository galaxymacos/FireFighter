using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI waterText;
    [SerializeField] private TextMeshProUGUI windowOnFire;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI highestScoreText;
    
    private bool loadingFinished;
    private GameManager GameManager;

    private float lifeTimeRemain;

    // Use this for initialization
    void Start() {
        lifeTimeRemain = lifeTime;
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update() {
        lifeTimeRemain -= Time.deltaTime;
        if (!loadingFinished && lifeTimeRemain <= 0) {
            loadingFinished = true;
            loadingText.transform.localScale = new Vector3(1, 1, 1);
            loadingText.GetComponent<Animator>().enabled = false;
            loadingText.text = "Finished";
            continueButton.SetActive(true);
        }
    }

    private void OnEnable() {
        Invoke("UpdateText",0.1f);
    }

    void UpdateText() {
        float totalScore = 0;
        damageText.text = "Damage: - " + (int)GameManager.damage + " * " + (int)GameManager.scorePerDamagePercentage + " = " +
                          (int)(GameManager.damage * GameManager.scorePerDamagePercentage);
        waterText.text = "Water Remain: + " + (int)GameManager.water + "% * " + (int)GameManager.scorePerWaterPercentage + " = " +
                         (int)GameManager.water * GameManager.scorePerWaterPercentage;

        totalScore -= GameManager.damage * GameManager.scorePerDamagePercentage;
        totalScore += GameManager.water * GameManager.scorePerWaterPercentage;
        float windowsScore = 0;

        foreach (bool curWindow in GameManager.hasBeenOnFired) {
            if (!curWindow) {
                windowsScore += 500;
            }
        }

        windowOnFire.text = "Windows haven't got on fire:" + (int)(windowsScore / 500) + " * " + 500 + " = " +
                            (int)windowsScore;

        totalScore += windowsScore;
        
        float gameTime = GameManager.second + GameManager.minute * 60;
        print("Game time is "+GameManager.second);
        float averagePassTime = GameManager.LevelInfos[GameManager.currentLevel].AveragePassTime;

        timeText.text = "Time: (" + (int)averagePassTime + " / " +
                        (int)gameTime + ") * " + 500 + " = " + (int)(averagePassTime / gameTime * 500);

        totalScore += averagePassTime / gameTime * 500;
        totalScoreText.text = "Total score: " + (int)totalScore;

        if (GameManager.highestScores[GameManager.currentLevel] > totalScore) {
            highestScoreText.text = "Highest score: " + GameManager.highestScores[GameManager.currentLevel];
        }
        else {    // You break the highest score
            GameManager.highestScores[GameManager.currentLevel] = (int) totalScore;
            highestScoreText.text = "Highest score: " + (int)totalScore;
        }
        
        GameManager.UpdatePlayerDataString();
    }
}