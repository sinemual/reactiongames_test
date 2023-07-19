using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class DisposeLevelSystem : IEcsRunSystem
    {
        private SharedData _data;
        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;

        //private EcsFilter<SpawnLevelRequest> _spawnRequestFilter;
        private EcsFilter<RestartLevelRequest> _restartRequestFilter;
        private EcsFilter<DisposeLevelRequest> _disposeRequestFilter;

        private EcsFilter<CurrentLevelTag> _currentLevelFilter;
        private EcsFilter<FromCurrentLevelTag> _fromCurrentLevelFilter;

        public void Run()
        {
            if (!_currentLevelFilter.IsEmpty() && (!_restartRequestFilter.IsEmpty() || !_disposeRequestFilter.IsEmpty()))
                DespawnCurrentLevel();

            foreach (var req in _disposeRequestFilter)
                _disposeRequestFilter.GetEntity(req).Del<DisposeLevelRequest>();
        }

        private void DespawnCurrentLevel()
        {
            Debug.Log($"DespawnCurrentLevel()");
            foreach (var fromCurrentLevel in _fromCurrentLevelFilter)
                _prefabFactory.Despawn(ref _fromCurrentLevelFilter.GetEntity(fromCurrentLevel));
            
            foreach (var fromCurrentLevel in _fromCurrentLevelFilter)
                _fromCurrentLevelFilter.GetEntity(fromCurrentLevel).Destroy();
            
            foreach (var currentLevel in _currentLevelFilter)
                _prefabFactory.Despawn(ref _currentLevelFilter.GetEntity(currentLevel));
        }
    }

    internal struct FromCurrentLevelTag
    {
    }

    internal struct DisposeLevelRequest : IEcsIgnoreInFilter
    {
    }
}