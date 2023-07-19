using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class LevelCompleteCheckSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<SavedUnitEvent> _eventFilter;
        private EcsFilter<PlayerUnitProvider, SavedState> _savedFilter;

        public void Run()
        {
            foreach (var idx in _eventFilter)
            {
                if (_savedFilter.GetEntitiesCount() >= _data.PlayerData.NeededSaveUnitsCount)
                {
                    _ui.LevelCompleteScreen.SetShowState(true);
                    _world.NewEntity().Get<LevelCompleteEvent>();
                    _data.RuntimeData.PauseGame(true);
                }
            }
        }
    }

    public struct LevelFailedEvent : IEcsIgnoreInFilter
    {
    }
}