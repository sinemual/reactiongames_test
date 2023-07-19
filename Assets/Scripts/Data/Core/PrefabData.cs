using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client.Data
{
    [ShowOdinSerializedPropertiesInInspector]
    public class PrefabData
    {
        public GameObject TurretBulletPrefab;
        
        [Header("Pools")] 
        public Dictionary<GameObject, int> PoolPrefabData;
    }
}