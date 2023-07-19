using Client.Data.Core;
using Leopotam.Ecs;

namespace Client
{
    public class LevelCompleteScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private UserInterfaceEventBus _eventBus;
        
        public void Init()
        {
            _eventBus.LevelCompleteScreen.NextLevelButtonTap += () =>
            {
                _data.PlayerData.CurrentLevelIndex++;
                _data.PlayerData.EventLevelIndex++;
                _world.NewEntity().Get<DisposeLevelRequest>();
                _world.NewEntity().Get<SpawnLevelRequest>();
                _ui.LevelCompleteScreen.SetShowState(false);
                _data.RuntimeData.PauseGame(false);
            };
        }
    }
}