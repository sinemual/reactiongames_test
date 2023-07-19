using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class KamikazeDestroySystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private CameraService _cameraService;

        private EcsFilter<EnemyKamikazeProvider, ChaseState, MovingCompleteEvent> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var chaseEntity = ref entity.Get<ChaseState>().ChaseEntity;

                chaseEntity.Get<HitRequest>().Damage = entity.Get<DamageStat>().Value;
                entity.Get<DeadRequest>();
            }
        }
    }
}