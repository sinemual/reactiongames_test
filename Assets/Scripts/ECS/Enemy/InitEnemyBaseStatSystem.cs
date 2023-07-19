using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class InitEnemyBaseStatSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private CameraService _cameraService;

        private EcsFilter<EnemyProvider>.Exclude<InitedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var enemyType = ref entity.Get<EnemyProvider>().EnemyType;

                entity.Get<DamageStat>().Value = _data.BalanceData.EnemyStats[enemyType].Damage;
                entity.Get<SpeedStat>().Value = _data.BalanceData.EnemyStats[enemyType].Speed;
                entity.Get<RangeStat>().Value = _data.BalanceData.EnemyStats[enemyType].Range;
            }
        }
    }

    public struct PlayerUnitDetectedEvent
    {
        public GameObject PlayerUnitGo;
    }
}