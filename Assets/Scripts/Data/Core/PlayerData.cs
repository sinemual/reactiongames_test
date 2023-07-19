using System;
using System.Collections.Generic;
using System.Numerics;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Data
{
    [ShowOdinSerializedPropertiesInInspector]
    public class PlayerData
    {
        public int CurrentLevelIndex;
        public int EventLevelIndex;
        public int LastLevelIndex;

        public int SavedUnitsCounter;
        public int NeededSaveUnitsCount;
        public int SelectedUnitNumber;

        public Dictionary<int, PlayerUnitSaveData> PlayerUnitsOnLevel;
        public Dictionary<int, EnemyUnitSaveData> EnemyUnitsOnLevel;

        public PlayerData()
        {
            CurrentLevelIndex = 0;
            EventLevelIndex = 0;
            LastLevelIndex = 0;
        }

        public PlayerData(PlayerData data)
        {
            CurrentLevelIndex = data.CurrentLevelIndex;
            EventLevelIndex = data.EventLevelIndex;
            LastLevelIndex = data.LastLevelIndex;
            SavedUnitsCounter = data.SavedUnitsCounter;
            NeededSaveUnitsCount = data.NeededSaveUnitsCount;
            SelectedUnitNumber = data.SelectedUnitNumber;
            PlayerUnitsOnLevel = new Dictionary<int, PlayerUnitSaveData>(data.PlayerUnitsOnLevel);
            EnemyUnitsOnLevel = new Dictionary<int, EnemyUnitSaveData>(data.EnemyUnitsOnLevel);
        }
    }

    [Serializable]
    public class PlayerUnitSaveData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public float Health;
        public bool IsSaved;
        public bool IsDead;
    }

    [Serializable]
    public class EnemyUnitSaveData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public bool IsDead;
    }
}