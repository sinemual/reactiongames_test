using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class CompleteInitEnemySystem : IEcsRunSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private CameraService _cameraService;

        private EcsFilter<EnemyProvider>.Exclude<InitedMarker> _filter;

        public void Run()
        {
            foreach (var idx in _filter)
            {
                ref var entity = ref _filter.GetEntity(idx);
                entity.Get<InitedMarker>();
            }
        }
    }
}