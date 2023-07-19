using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class InitPlayerUnitStatsSystem : IEcsRunSystem
    {
        private SharedData _data;

        private EcsFilter<PlayerUnitProvider>.Exclude<InitedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);

                entity.Get<SpeedStat>().Value = _data.BalanceData.PlayerUnitMovementSpeed;
                entity.Get<HealthStat>().Value = _data.BalanceData.PlayerUnitHealth;
                entity.Get<BaseHealthStat>().Value = _data.BalanceData.PlayerUnitHealth;
            }
        }
    }

    public struct BaseHealthStat
    {
        public float Value;
    }
}