using System;
using System.Collections.Generic;
using Client.Data;
using Client.Data.Core;
using Data.Base;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/BalanceData", fileName = "BalanceData")]
    public class BalanceData : BaseDataSO
    {
        [Header("Player")] 
        public float PlayerUnitMovementSpeed;
        public float PlayerUnitHealth;
        public float CharactersRotateSpeed;

        [Header("Enemies")] 
        public Dictionary<EnemyType, EnemyStats> EnemyStats;
    }

    [Serializable]
    public class EnemyStats
    {
        public float Damage;
        public float Speed;
        public float Range;
    }
}