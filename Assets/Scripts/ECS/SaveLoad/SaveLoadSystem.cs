using System;
using System.Collections.Generic;
using Client.Data.Core;
using Data;
using Leopotam.Ecs;

namespace Client
{
    public class SaveLoadSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private EcsFilter<SaveRequest> _saveRequestFilter;
        private EcsFilter<LoadRequest> _loadRequestFilter;
        private EcsFilter<PlayerUnitProvider, SaveProvider> _playerFilter;
        private EcsFilter<EnemyProvider, SaveProvider> _enemyFilter;

        public void Run()
        {
            foreach (var request in _saveRequestFilter)
            {
                ref var requestEntity = ref _saveRequestFilter.GetEntity(request);
                ref var slotNum = ref requestEntity.Get<SaveRequest>().SlotNum;

                _data.PlayerData.PlayerUnitsOnLevel = new Dictionary<int, PlayerUnitSaveData>();
                _data.PlayerData.EnemyUnitsOnLevel = new Dictionary<int, EnemyUnitSaveData>();

                foreach (var idx in _playerFilter)
                {
                    ref var entity = ref _playerFilter.GetEntity(idx);
                    ref var id = ref entity.Get<SpawnData>().Id;
                    ref var entityGo = ref entity.Get<GameObjectProvider>().Value;

                    var unitSaveData = new PlayerUnitSaveData();
                    unitSaveData.Position = entityGo.transform.position;
                    unitSaveData.Rotation = entityGo.transform.rotation;
                    unitSaveData.IsSaved = entity.Has<SavedState>();
                    unitSaveData.Health = entity.Get<HealthStat>().Value;
                    unitSaveData.IsDead = entity.Has<DeadState>();
                    if(_data.PlayerData.PlayerUnitsOnLevel.ContainsKey(id))
                        _data.PlayerData.PlayerUnitsOnLevel[id] = unitSaveData;
                    else
                        _data.PlayerData.PlayerUnitsOnLevel.Add(id, unitSaveData);
                }
                
                foreach (var idx in _enemyFilter)
                {
                    ref var entity = ref _enemyFilter.GetEntity(idx);
                    ref var id = ref entity.Get<SpawnData>().Id;
                    ref var entityGo = ref entity.Get<GameObjectProvider>().Value;

                    var unitSaveData = new EnemyUnitSaveData();
                    unitSaveData.Position = entityGo.transform.position;
                    unitSaveData.Rotation = entityGo.transform.rotation;
                    unitSaveData.IsDead = entity.Has<DeadState>();
                    if(_data.PlayerData.EnemyUnitsOnLevel.ContainsKey(id))
                        _data.PlayerData.EnemyUnitsOnLevel[id] = unitSaveData;
                    else
                        _data.PlayerData.EnemyUnitsOnLevel.Add(id, unitSaveData);
                }
                
                _data.SavePlayerData[slotNum] = new PlayerData(_data.PlayerData);
                _data.SaveData();
                requestEntity.Del<SaveRequest>();
            }

            foreach (var request in _loadRequestFilter)
            {
                ref var requestEntity = ref _loadRequestFilter.GetEntity(request);
                ref var slotNum = ref requestEntity.Get<LoadRequest>().SlotNum;

                _data.PlayerData = new PlayerData(_data.SavePlayerData[slotNum]);
                
                _world.NewEntity().Get<DisposeLevelRequest>();
                _world.NewEntity().Get<SpawnLevelRequest>();
                _world.NewEntity().Get<LoadUnitsRequest>();
                
                requestEntity.Del<LoadRequest>();
            }
        }
    }

    internal struct LoadUnitsRequest
    {
    }
}