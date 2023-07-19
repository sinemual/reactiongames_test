using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class NextSelectPlayerUnitSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;

        private EcsFilter<SelectNextUnitRequest> _requestFilter;

        public void Run()
        {
            foreach (var idx in _requestFilter)
            {
                ref var entity = ref _requestFilter.GetEntity(idx);

                _data.PlayerData.SelectedUnitNumber++;

                if (_data.PlayerData.SelectedUnitNumber >= _data.PlayerData.NeededSaveUnitsCount)
                    _data.PlayerData.SelectedUnitNumber = 0;
                
                _world.NewEntity().Get<SelectUnitRequest>().Number = _data.PlayerData.SelectedUnitNumber;

                entity.Del<SelectNextUnitRequest>();
            }
        }
    }
}