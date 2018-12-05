using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo",menuName = "ScriptableObject")]
public class LevelInfo : ScriptableObject {
    public float damageSpeed;
    public float waterLossSpeed;
    public int startFire;
    public int maxFireNum;
    public int FireLimitedInScene;
    public float AveragePassTime;
    internal float highScore;
}
