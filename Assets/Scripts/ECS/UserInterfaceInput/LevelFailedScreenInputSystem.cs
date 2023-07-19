using Client.Data;
using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class LevelFailedScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private UserInterfaceEventBus _eventBus;
        
        public void Init()
        {
            _eventBus.LevelFailedScreen.RestartLevelButtonTap += () =>
            {
                _world.NewEntity().Get<RestartLevelRequest>();
                _ui.LevelFailedScreen.SetShowState(false);
                _data.RuntimeData.PauseGame(false);
            };
        }
    }
}