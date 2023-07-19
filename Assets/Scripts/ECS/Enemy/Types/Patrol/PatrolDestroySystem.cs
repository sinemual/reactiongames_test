using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class PatrolDestroySystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;

        private PrefabFactory _prefabFactory;
        private CameraService _cameraService;

        private EcsFilter<EnemyPatrolProvider, OnTriggerEnterEvent> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var entityCollision = ref entity.Get<OnTriggerEnterEvent>();

                if (entityCollision.Collider.gameObject.TryGetComponent(out MonoEntity mono))
                {
                    if (mono.Entity.Has<PlayerUnitProvider>())
                        mono.Entity.Get<HitRequest>().Damage = entity.Get<DamageStat>().Value;
                }
                
                _prefabFactory.Despawn(ref entity);
            }
        }
    }
}