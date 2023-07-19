using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Data
{
    [CreateAssetMenu(menuName = "GameData/LevelsData", fileName = "LevelsData")]
    public class LevelsData : ScriptableObject
    {
        public List<GameObject>  Levels;
        public int AlwaysLoadLevelId;
    }
}