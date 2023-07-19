using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class CompleteInitPlayerUnitSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private UserInterfaceEventBus _uiEventBus;

        private EcsFilter<PlayerUnitProvider>.Exclude<InitedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var entityIdx = ref entity.Get<SpawnData>().Id;
                ref var entityProvider = ref entity.Get<PlayerUnitProvider>();

                _data.PlayerData.NeededSaveUnitsCount++;
                _uiEventBus.UnitPanel.OnInitUnitsPanelAmount(_data.PlayerData.NeededSaveUnitsCount);
                _uiEventBus.UnitPanel.OnInitUnitPanel(entityIdx);
                entityProvider.Number = entityIdx;
                _world.NewEntity().Get<SelectNextUnitRequest>();
                entity.Get<InitedMarker>();
            }
        }
    }
}