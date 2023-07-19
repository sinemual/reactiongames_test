using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class LoadUnitsSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private EcsFilter<LoadUnitsRequest> _loadRequestFilter;
        private EcsFilter<PlayerUnitProvider, InitedMarker> _playerFilter;
        private EcsFilter<EnemyProvider, InitedMarker> _enemyFilter;

        public void Run()
        {
            foreach (var request in _loadRequestFilter)
            {
                ref var requestEntity = ref _loadRequestFilter.GetEntity(request);

                foreach (var idx in _playerFilter)
                {
                    ref var entity = ref _playerFilter.GetEntity(idx);
                    ref var id = ref entity.Get<SpawnData>().Id;
                    ref var entityGo = ref entity.Get<GameObjectProvider>().Value;

                    var unitSaveData = _data.PlayerData.PlayerUnitsOnLevel[id];
                    entityGo.transform.SetPositionAndRotation(unitSaveData.Position, unitSaveData.Rotation);

                    if (unitSaveData.IsSaved)
                        entity.Get<SavedState>();

                    if (unitSaveData.IsDead)
                        entity.Get<DeadState>();

                    entity.Get<HealthStat>().Value = unitSaveData.Health;
                }

                foreach (var idx in _enemyFilter)
                {
                    ref var entity = ref _enemyFilter.GetEntity(idx);
                    ref var id = ref entity.Get<SpawnData>().Id;
                    ref var entityGo = ref entity.Get<GameObjectProvider>().Value;

                    var unitSaveData = _data.PlayerData.EnemyUnitsOnLevel[id];
                    entityGo.transform.SetPositionAndRotation(unitSaveData.Position, unitSaveData.Rotation);

                    if (unitSaveData.IsDead)
                        entity.Get<DeadState>();

                    _world.NewEntity().Get<SelectUnitRequest>().Number = _data.PlayerData.SelectedUnitNumber;
                    
                    if (requestEntity.IsAlive())
                        requestEntity.Del<LoadUnitsRequest>();
                }
            }
        }
    }
}