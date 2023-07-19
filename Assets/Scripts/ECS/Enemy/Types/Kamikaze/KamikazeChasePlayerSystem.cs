using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class KamikazeChasePlayerSystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private CameraService _cameraService;

        private EcsFilter<EnemyKamikazeProvider, PlayerUnitDetectedEvent>.Exclude<ChaseState, DeadState> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var enemyKamikazeProvider = ref entity.Get<EnemyKamikazeProvider>();
                ref var speed = ref entity.Get<SpeedStat>().Value;
                ref var targetGo = ref entity.Get<PlayerUnitDetectedEvent>().PlayerUnitGo;

                entity.Del<TransformMoving>();
                entity.Get<TransformMagnetMoving>() = new TransformMagnetMoving()
                {
                    Accuracy = 0.1f,
                    Speed = speed,
                    Target = targetGo
                };
                
                entity.Get<ChaseState>().ChaseEntity = targetGo.GetComponent<MonoEntity>().Entity;
            }
        }
    }
}