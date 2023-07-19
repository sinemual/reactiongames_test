using Client.DevTools.MyTools;
using Data.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client.Data
{
    [CreateAssetMenu(menuName = "GameData/StaticData", fileName = "StaticData")]
    public class StaticData : BaseDataSO
    {
        [FoldoutGroup("Prefabs")] public PrefabData PrefabData;
        [FoldoutGroup("Levels")] public LevelsData LevelsData;
        
        [FoldoutGroup("Tags")] public string ExitTag;
        
        [FoldoutGroup("Layer")] public LayerMask RaycastMask;
        [FoldoutGroup("Layer")] public LayerMask PlayerMask;
        
        public int GetPlayerLayer => Utility.ToLayer(PlayerMask);
    }
}