using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class PlayerUnitExitSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private GameUI _ui;

        private CameraService _cameraService;
        private UserInterfaceEventBus _uiEventBus;

        private EcsFilter<LevelExitProvider, OnTriggerEnterEvent> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var entityCollision = ref entity.Get<OnTriggerEnterEvent>();

                if (entityCollision.Collider)
                    if (entityCollision.Collider.gameObject.TryGetComponent(out MonoEntity mono))
                    {
                        mono.Entity.Get<SavedState>();
                        mono.Entity.Get<SavedUnitEvent>();
                        entityCollision.Collider.gameObject.SetActive(false);
                        _data.PlayerData.SavedUnitsCounter++;
                        if (_data.PlayerData.SavedUnitsCounter < _data.PlayerData.NeededSaveUnitsCount + 1)
                            _world.NewEntity().Get<SelectNextUnitRequest>();
                    }
            }
        }
    }

    public struct SavedUnitEvent : IEcsIgnoreInFilter
    {
    }

    public struct SavedState : IEcsIgnoreInFilter
    {
    }
}