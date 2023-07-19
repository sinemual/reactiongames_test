using Client.Data.Core;
using Client.Infrastructure.Services;
using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    public class PauseSystem : IEcsRunSystem
    {
        private SharedData _data;
        private EcsWorld _world;
        private GameUI _ui;
        
        private CameraService _cameraService;
        
        public void Run()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_data.RuntimeData.IsPause)
                {
                    _ui.PauseScreen.SetShowState(true);
                    _data.RuntimeData.PauseGame(true);
                }
                else
                {
                    _data.RuntimeData.PauseGame(false);
                    _ui.PauseScreen.SetShowState(false);
                }
            }
        }
    }
}