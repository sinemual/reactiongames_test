using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class SelectPlayerUnitSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private CameraService _cameraService;
        private UserInterfaceEventBus _uiEventBus;

        private EcsFilter<SelectUnitRequest> _requestFilter;
        private EcsFilter<PlayerUnitProvider, SaveProvider> _unitFilter;

        public void Run()
        {
            foreach (var idx in _requestFilter)
            {
                ref var entity = ref _requestFilter.GetEntity(idx);
                ref var number = ref entity.Get<SelectUnitRequest>().Number;

                foreach (var unit in _unitFilter)
                {
                    ref var unitEntity = ref _unitFilter.GetEntity(unit);
                    ref var unitProvider = ref unitEntity.Get<PlayerUnitProvider>();
                    unitProvider.Pointer.SetActive(false);
                    unitEntity.Del<SelectedUnitTag>();

                    if (unitProvider.Number == number)
                    {
                        if (unitEntity.Has<DeadState>() || unitEntity.Has<SavedState>() || unitEntity.Has<SelectedUnitTag>())
                            _world.NewEntity().Get<SelectNextUnitRequest>();
                        
                        ref var unitGo = ref unitEntity.Get<GameObjectProvider>();
                        unitProvider.Pointer.SetActive(true);
                        _cameraService.SetTarget(unitGo.Value.transform, unitGo.Value.transform);
                        unitEntity.Get<SelectedUnitTag>();
                        _data.PlayerData.SelectedUnitNumber = number;
                        _uiEventBus.UnitPanel.OnSelectNextUnit(number);
                    }
                }
                
                entity.Del<SelectUnitRequest>();
            }
        }
    }
    
}