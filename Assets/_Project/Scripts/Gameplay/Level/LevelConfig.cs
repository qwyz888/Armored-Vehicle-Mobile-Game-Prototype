using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Gameplay/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [Serializable]
    public class LevelData
    {
        public string LevelName;
        public float Length = 200f; // meters to travel
        public float EnemySpawnInterval = 2f; // seconds between spawn attempts
        public int MaxConcurrentEnemies = 10;
        public float EnemyHpMin = 50f;
        public float EnemyHpMax = 100f;
    }

    public LevelData[] Levels;

    public LevelData Get(int index)
    {
        if (Levels == null || Levels.Length == 0) return null;
        if (index < 0) index = 0;
        if (index >= Levels.Length) index = Levels.Length - 1;
        return Levels[index];
    }
}
