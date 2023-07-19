using Client.Data;
using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;

namespace Client
{
    public class InitGameSystem : IEcsInitSystem
    {
        private SharedData _data;
        private GameUI _ui;
        private EcsWorld _world;
        
        public void Init()
        {
            _world.NewEntity().Get<SpawnLevelRequest>();
        }
    }
}