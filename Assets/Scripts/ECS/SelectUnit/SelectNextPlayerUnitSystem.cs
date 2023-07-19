using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class SelectNextPlayerUnitSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private CameraService _cameraService;
        
        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _world.NewEntity().Get<SelectNextUnitRequest>();
            }
        }
    }
}
