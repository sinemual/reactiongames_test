using Client.Data;
using Client.Data.Core;
using Client.ECS.CurrentGame.Mining;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class PlayerUnitTakeDamageSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private CameraService _cameraService;
        private UserInterfaceEventBus _uiEventBus;

        private EcsFilter<PlayerUnitProvider, HitRequest> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var damage = ref entity.Get<HitRequest>().Damage;
                ref var health = ref entity.Get<HealthStat>().Value;
                ref var baseHealth = ref entity.Get<BaseHealthStat>().Value;
                ref var number = ref entity.Get<PlayerUnitProvider>().Number;

                health -= damage;
                if (health <= 0)
                {
                    entity.Get<DeadRequest>();
                    entity.Get<HitEvent>();
                    _world.NewEntity().Get<SelectNextUnitRequest>();
                }
                
                _ui.EventBus.UnitPanel.OnChangeUnitHealth(number, health / baseHealth);
                
                _cameraService.Shake();

                entity.Del<HitRequest>();
            }
        }
    }
}