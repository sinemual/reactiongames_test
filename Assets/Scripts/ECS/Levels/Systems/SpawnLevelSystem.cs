using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class SpawnLevelSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private PrefabFactory _prefabFactory;

        private EcsFilter<SpawnLevelRequest> _createNewLevelRequestFilter;
        private EcsFilter<RestartLevelRequest> _restartLevelRequestFilter;

        public void Run()
        {
            foreach (var idx in _createNewLevelRequestFilter)
            {
                CreateNextLevel();
                _createNewLevelRequestFilter.GetEntity(idx).Del<SpawnLevelRequest>();
            }
            
            foreach (var idx in _restartLevelRequestFilter)
            {
                RestartLevel();
                _restartLevelRequestFilter.GetEntity(idx).Del<RestartLevelRequest>();
            }
        }

        private void CreateNextLevel()
        {
            Debug.Log($"CreateNextLevel");
            if (_data.PlayerData.EventLevelIndex > _data.StaticData.LevelsData.Levels.Count - 1)
            {
                _data.PlayerData.CurrentLevelIndex = Random.Range(0, _data.StaticData.LevelsData.Levels.Count);
                    /*while (_data.PlayerData.CurrentLevelIndex == _data.RuntimeData.LastLevelIndex)
                        _data.PlayerData.CurrentLevelIndex = Random.Range(0, _data.StaticData.LevelsData.Levels.Count);*/
            }
            
            _data.PlayerData.LastLevelIndex = _data.PlayerData.CurrentLevelIndex;

            EcsEntity entity = _prefabFactory.Spawn(_data.StaticData.LevelsData.Levels[_data.PlayerData.CurrentLevelIndex].gameObject, Vector3.zero,
                Quaternion.identity);

            entity.Get<CurrentLevelTag>();
            
            Debug.Log($"CreateLevel level {_data.PlayerData.CurrentLevelIndex}");
        }

        private void RestartLevel()
        {
            _data.PlayerData.CurrentLevelIndex = _data.PlayerData.LastLevelIndex;
            CreateNextLevel();
        }
    }

    internal struct LoadLevelRequest
    {
    }
}