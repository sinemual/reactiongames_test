using Client.Data.Core;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class PauseScreenInputSystem : IEcsInitSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private UserInterfaceEventBus _eventBus;
        
        public void Init()
        {
            _eventBus.PauseScreen.ContinueButtonTap += () =>
            {
                _data.RuntimeData.PauseGame(false);
                _ui.PauseScreen.SetShowState(false);
            };
            
            _eventBus.PauseScreen.SaveButtonTap += (num) =>
            {
                _data.RuntimeData.PauseGame(false);
                _world.NewEntity().Get<SaveRequest>().SlotNum = num;
                _ui.PauseScreen.SetShowState(false);
            };
            
            _eventBus.PauseScreen.LoadButtonTap += (num) =>
            {
                _data.RuntimeData.PauseGame(false);
                _world.NewEntity().Get<LoadRequest>().SlotNum = num;
                _ui.PauseScreen.SetShowState(false);
            };
        }
    }
}