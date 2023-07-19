using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class UnitsScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private UserInterfaceEventBus _eventBus;
        
        public void Init()
        {
            _eventBus.UnitsScreen.SelectUnitButtonTap += (num) =>
            {
                _world.NewEntity().Get<SelectUnitRequest>().Number = num;
            };
        }
    }
}