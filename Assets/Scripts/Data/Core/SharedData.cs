using System.Collections.Generic;
using System.IO;
using Client.DevTools.MyTools;
using Client.Infrastructure.Services;
using Data;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Client.Data.Core
{
    public class SharedData : SerializedMonoBehaviour
    {
        public StaticData StaticData;
        public BalanceData BalanceData;
        public PlayerData PlayerData;
        public Dictionary<int, PlayerData> SavePlayerData; // slot, data
        public RuntimeData RuntimeData;

        private ISaveLoadService _saveLoadService;

        public void ManualStart(ISaveLoadService saveLoadService) // from Game.cs
        {
            Debug.Log(Utility.GetDataPath());

            _saveLoadService = saveLoadService;

            RuntimeData = new RuntimeData();
            PlayerData = new PlayerData();
            
            SavePlayerData = new Dictionary<int, PlayerData>();
            for (int i = 0; i < 3; i++)
                SavePlayerData.Add(i, new PlayerData());

            LoadData();
        }

        public void SaveData()
        {
            foreach (var saveData in SavePlayerData)
                _saveLoadService.Save(saveData.Value, saveData.Key.ToString());
        }

        private void LoadData()
        {
            for (int i = 0; i < SavePlayerData.Count; i++)
                SavePlayerData[i] = _saveLoadService.Load<PlayerData>(i.ToString());
        }

#if UNITY_EDITOR
        [ExecuteInEditMode]
        [MenuItem("Tools/DeleteAllGameData")]
        public static void DeleteAllGameData()
        {
            if (Directory.Exists(Utility.GetDataPath()))
                Directory.Delete(Utility.GetDataPath(), true);
        }
#endif
        /*private void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }*/
    }
}