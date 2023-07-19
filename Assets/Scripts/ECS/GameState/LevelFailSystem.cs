using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class LevelFailSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private SharedData _data;
        private GameUI _ui;

        private EcsFilter<DeadEvent> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                ref var deadEntity = ref entity.Get<DeadEvent>().Entity;

                if (deadEntity.Has<PlayerUnitProvider>())
                {
                    _ui.LevelFailedScreen.SetShowState(true);
                    _world.NewEntity().Get<LevelFailedEvent>();
                    _data.RuntimeData.PauseGame(true);
                }
            }
        }
    }
}